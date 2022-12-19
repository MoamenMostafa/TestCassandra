using Microsoft.AspNetCore.Mvc;
using Cassandra;
using Cassandra.Mapping;
using TestCassandra.Models;

namespace TestCassandra.Controllers
{
    public class logsController : Controller
    {
        private ICluster _cluster;
        private Cassandra.ISession _session;
        private IMapper _mapper;
        private IConfiguration _configuration;

        public logsController(IConfiguration _config)
        {
            _configuration = _config;
            _cluster = Cluster.Builder().AddContactPoint(_config["ConnectionStrings:DefaultConnection"]).Build();
            _session = _cluster.Connect("testkeyspace");
            _mapper = new Mapper(_session);
        }

        [HttpPost]
        [Route("api/AddBySessionId")]
        public IActionResult AddBySessionId(int sessionId)
        {
            createLogs(sessionId);
            return Ok();
        }

        [HttpPost]
        [Route("api/Addlog")]
        public IActionResult Addlog(int sessionId , string log)
        {
            var logTable = new logsTable();
            logTable.SessionId = sessionId; logTable.Log = log;
            _mapper.Insert(logTable);
            return Ok();
        }

        [HttpGet]
        [Route("api/Getlogs")]
        public IActionResult GetAllLogs()
        {
            var logs = _mapper.Fetch<logsTable>();
            return Ok(logs.ToList());
        }

        [HttpDelete]
        [Route("api/DeleteSession")]
        public IActionResult DeleteSession(int sessionId)
        {
            _mapper.Execute(($"delete from logsTable where sessionId = {sessionId}"));
            return Ok();
        }

        [HttpGet]
        [Route("api/GetBySessionId")]
        public IActionResult GetBySessionId(int sessionId)
        {
            var logs = _session.Execute($"select * from logsTable where sessionId = {sessionId}");
            return Ok(logs.ToList());
        }

        private void createLogs(int sessionId)
        {
            
            for (int i = 0; i < 1000000; i++)
            {
                var logsTable = new logsTable();
                logsTable.SessionId = sessionId; logsTable.Log = $"Log test {i}";
                _mapper.Insert(logsTable);
            }
        }
    }
}
