using System.Text.Json.Serialization;

namespace PRSproject.Models {

    public class RequestLine {

        public int Id {get; set; }

        public int RequestId { get; set; }
        //prevents the calling loop between requests and request lines by not printing any data from this part of the cycle
        [JsonIgnore] 
        public virtual Request? Request { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int Quantity { get; set; }

    }
}
