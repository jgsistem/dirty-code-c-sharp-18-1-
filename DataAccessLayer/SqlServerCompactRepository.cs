using BusinessLayer;

namespace DataAccessLayer
{
    public class SqlServerCompactRepository : IRepository
	{
		public int SaveSpeaker(Speaker speaker)
		{
			return 1;
		}
	}
}
