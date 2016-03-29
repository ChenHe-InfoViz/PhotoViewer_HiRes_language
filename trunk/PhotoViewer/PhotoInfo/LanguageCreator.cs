using database;

namespace PhotoConstruction
{
    class LanguageCreator: PhotoCreator
    {
        public LanguageCreator()
            : base(new LanTable())
        {
 
        }
    }
}
