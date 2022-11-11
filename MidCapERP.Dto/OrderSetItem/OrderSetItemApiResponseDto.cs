﻿using System.Text.Json.Serialization;

namespace MidCapERP.Dto.OrderSetItem
{
    public class OrderSetItemApiResponseDto
    {
        public long OrderSetItemId { get; set; }

        [JsonIgnore]
        public long OrderId { get; set; }

        [JsonIgnore]
        public long OrderSetId { get; set; }

        public int SubjectTypeId { get; set; }
        public long SubjectId { get; set; }
        public string ProductImage { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Depth { get; set; }
        public int Quantity { get; set; }
        public string ProductTitle { get; set; }
        public string ModelNo { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public string Comment { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public decimal? ProvidedMaterial { get; set; }
        public int Status { get; set; }
        public bool IsItemReceived { get; set; }
    }
}