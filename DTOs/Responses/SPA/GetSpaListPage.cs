namespace HwaidakAPI.DTOs.Responses.SPA
{
    public class GetSpaListPage
    {
        public MainResponse PageDetails { get; set; }
        public List<GetSPA> sPAs { get; set; }
    }
}
