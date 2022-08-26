
using CleanArchitecture.Domain.Common;

namespace CleanArchitecture.Domain
{
    public class Video : BaseDomainModel
    {
        //este es el constructor
        public Video() 
        {
            Actores = new List<Actor>();
        }

        public string? Nombre { get; set; }

        //Ancla para clave
        // Cuando le damos la propiedad virtual significa que puede ser
        // sobreescrita por una clase derivada en un futuro       

        public virtual Streamer? Streamer { get; set; }

        // clave FK
        public int? StreamerId { get; set; }

        public ICollection<Actor> Actores { get; set; }

        public virtual Director Director { get; set; }



    }
}
