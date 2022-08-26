
namespace CleanArchitecture.Domain.Common
{
    //Importante que esta clase sea de tipo abstracta para que no permita instanciar
    public abstract class BaseDomainModel
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string? LastModifiedBy { get; set; }



    }
}
