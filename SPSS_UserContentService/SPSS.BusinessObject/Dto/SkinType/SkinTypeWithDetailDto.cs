namespace SPSS.BusinessObject.Dto.SkinType;

public class SkinTypeWithDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Routine { get; set; } // THÊM TRƯỜNG NÀY
}