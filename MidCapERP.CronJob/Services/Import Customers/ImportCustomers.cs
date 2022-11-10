using Microsoft.Extensions.DependencyInjection;
using MidCapERP.Core.Constants;
using MidCapERP.DataAccess.Interface;
using MidCapERP.DataAccess.UnitOfWork;
using MidCapERP.DataEntities.Models;
using MidCapERP.Dto;

namespace MidCapERP.CronJob.Services.Import_Customers
{
    public class ImportCustomers : IImportCustomers
    {
        private readonly CurrentUser _currentUser;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IUnitOfWorkDA _unitOfWorkDA;
        private readonly IWrkImportFilesDA _wrkImportFilesDA;

        public ImportCustomers(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            IServiceScope scope = _serviceScopeFactory.CreateScope();
            _currentUser = scope.ServiceProvider.GetRequiredService<CurrentUser>();
            _unitOfWorkDA = scope.ServiceProvider.GetRequiredService<IUnitOfWorkDA>();
            _wrkImportFilesDA = scope.ServiceProvider.GetRequiredService<IWrkImportFilesDA>();
        }

        public async Task ImportCustomersFromCsvFile(CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWorkDA.BeginTransactionAsync();
                var getWrkFiles = _wrkImportFilesDA.GetAll(cancellationToken).Result
                    .Where(x => x.FileType == "Customer" && x.Status == (int)FileUploadStatusEnum.Pending);

                //Process Start -Update Process start date in WrkImportFiles
                foreach (var item in getWrkFiles)
                {
                    item.ProcessStartDate = DateTime.Now;
                    item.UpdatedBy = _currentUser.UserId;
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedUTCDate = DateTime.UtcNow;
                    await _unitOfWorkDA.WrkImportFilesDA.Update(item, cancellationToken);
                }

                // Get Customer data from WrkImportCustomer Table
                if (getWrkFiles != null && getWrkFiles.ToList().Count > 0)
                {
                    var getWrkCustomers = _unitOfWorkDA.WrkImportCustomersDA.GetAll(cancellationToken).Result.Where(x => getWrkFiles.Select(c => c.WrkImportFileID).Contains(x.WrkImportFileID) && x.Status == (int)FileUploadStatusEnum.Pending).ToList();

                    if (getWrkCustomers != null && getWrkCustomers.Count > 0)
                    {
                        foreach (var item in getWrkCustomers)
                        {
                            try
                            {
                                //check Customer From customer table with mobile number
                                var getCustomer = _unitOfWorkDA.CustomersDA.GetAll(cancellationToken).Result.Where(z => z.PhoneNumber == item.PrimaryContactNumber).FirstOrDefault();

                                if (getCustomer == null)
                                {
                                    //Add Customer in customer Table
                                    var customer = WrkCustomerToCustomerTable(item);
                                    var createdCustomer = await _unitOfWorkDA.CustomersDA.CreateCustomers(customer, cancellationToken);

                                    //Add address in CustomerAddress Table for new created custmer
                                    var customerAddress = WrkCustomerToCustomerAddress(createdCustomer.CustomerId, item);
                                    await _unitOfWorkDA.CustomerAddressesDA.CreateCustomerAddress(customerAddress, cancellationToken);

                                    //Update status as completed on WrkImpotCustomers Table
                                    item.Status = 1;
                                    item.UpdatedBy = _currentUser.UserId;
                                    item.UpdatedDate = DateTime.Now;
                                    item.UpdatedUTCDate = DateTime.UtcNow;
                                }
                                else
                                {
                                    //Update status as already exists on WrkImpotCustomers Table
                                    item.Status = (int)FileUploadStatusEnum.AlreadyExists;
                                    item.UpdatedBy = _currentUser.UserId;
                                    item.UpdatedDate = DateTime.Now;
                                    item.UpdatedUTCDate = DateTime.UtcNow;
                                }
                            }
                            catch (Exception)
                            {
                                await _unitOfWorkDA.rollbackTransactionAsync();
                                //Update status as failed on WrkImpotCustomers Table
                                item.Status = (int)FileUploadStatusEnum.Failed;
                                item.UpdatedBy = _currentUser.UserId;
                                item.UpdatedDate = DateTime.Now;
                                item.UpdatedUTCDate = DateTime.UtcNow;
                                throw;
                            }
                            await _unitOfWorkDA.WrkImportCustomersDA.Update(item, cancellationToken);
                        }
                    }
                }
                else
                {
                    // Process Failed - Update Process Status failed in WrkImportFiles
                    foreach (var item in getWrkFiles)
                    {
                        item.Status = (int)FileUploadStatusEnum.Failed;
                        item.Failed = (int)FileUploadStatusEnum.Failed;
                        item.UpdatedBy = _currentUser.UserId;
                        item.UpdatedDate = DateTime.Now;
                        item.UpdatedUTCDate = DateTime.UtcNow;
                        await _unitOfWorkDA.WrkImportFilesDA.Update(item, cancellationToken);
                    }
                }
                // Process End - Update Process End date in WrkImportFiles
                foreach (var item in getWrkFiles)
                {
                    item.Success = 1;
                    item.Status = (int)FileUploadStatusEnum.Completed;
                    item.UpdatedBy = _currentUser.UserId;
                    item.ProcessEndDate = DateTime.Now;
                    item.UpdatedDate = DateTime.Now;
                    item.UpdatedUTCDate = DateTime.UtcNow;
                    await _unitOfWorkDA.WrkImportFilesDA.Update(item, cancellationToken);
                }
                await _unitOfWorkDA.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWorkDA.rollbackTransactionAsync();
                throw;
            }
        }

        public Customers WrkCustomerToCustomerTable(WrkImportCustomers wrkImportCustomers)
        {
            return new Customers()
            {
                AltPhoneNumber = wrkImportCustomers.AlternateContactNumber,
                FirstName = wrkImportCustomers.FirstName,
                LastName = wrkImportCustomers.LastName,
                EmailId = wrkImportCustomers.EmailID,
                GSTNo = wrkImportCustomers.GSTNo,
                PhoneNumber = wrkImportCustomers.PrimaryContactNumber,
                TenantId = _currentUser.TenantId,
                CustomerTypeId = (int)CustomerTypeEnum.Customer,
                CreatedBy = (int)wrkImportCustomers.CreatedBy,
                CreatedDate = DateTime.Now,
                CreatedUTCDate = DateTime.UtcNow,
            };
        }

        public CustomerAddresses WrkCustomerToCustomerAddress(long CustomerId, WrkImportCustomers wrkImportCustomers)
        {
            return new CustomerAddresses()
            {
                Area = wrkImportCustomers.Area,
                City = wrkImportCustomers.City,
                CustomerId = CustomerId,
                Landmark = wrkImportCustomers.Landmark,
                Street1 = wrkImportCustomers.Street1,
                Street2 = wrkImportCustomers.Stree2,
                State = wrkImportCustomers.State,
                ZipCode = wrkImportCustomers.ZipCode,
                CreatedBy = (int)wrkImportCustomers.CreatedBy,
                CreatedDate = DateTime.Now,
                CreatedUTCDate = DateTime.UtcNow,
                AddressType = "Home",
            };
        }
    }
}