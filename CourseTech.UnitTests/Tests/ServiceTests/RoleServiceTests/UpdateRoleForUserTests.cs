using Microsoft.SCP;
using Microsoft.SCP.Rpc.Generated;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace CourseTech.UnitTests.Tests.ServiceTests.RoleServiceTests
{
    public class UpdateRoleForUserTests : ISCPBatchBolt
    {
        public void Execute(SCPTuple tuple)
        {
        }

        public void FinishBatch(Dictionary<string, Object> parms)
        {
        }
    }
}