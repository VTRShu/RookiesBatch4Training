using System.ComponentModel.DataAnnotations;
namespace LibraryManagementBE.Repositories.Requests;

public class PagingRequest
{
    public int PageIndex { get; set; }
    [Required]
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int PageCount
    {
        get
        {
            var pageCount = (double)TotalRecords / PageSize;
            return (int)Math.Ceiling(pageCount);
        }
    }
}
