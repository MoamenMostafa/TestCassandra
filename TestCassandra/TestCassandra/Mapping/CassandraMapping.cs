using TestCassandra.Models;

namespace TestCassandra.Mapping
{
    public class CassandraMapping : Cassandra.Mapping.Mappings
    {
        public CassandraMapping()
        {
            For<logsTable>().TableName("logsTable")
            .Column(l => l.SessionId,s=>s.WithName("sessionId"))
            .Column(l => l.Log,l=>l.WithName("log"));
        }
    }
}
