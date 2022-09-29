namespace noCarbon.Core.Dtos;

public partial class CategoryDto
{
    public CategoryDto(int id)
    {
        this.Id = id;
    }
    public int Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] Icon { get; set; }
}
