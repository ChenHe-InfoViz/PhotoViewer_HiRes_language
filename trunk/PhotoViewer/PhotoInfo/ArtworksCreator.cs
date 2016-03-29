using database;

namespace PhotoConstruction
{
    class ArtworksCreator: PhotoCreator
    {
        public ArtworksCreator(): base(new ArtworksTable())
        {
 
        }
    }
}
