using Microsoft.Extensions.Configuration;
using release_tracker.LocalDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.BusinessLogic
{
    public class Factory
    {
        public static ReleaseFileFacade GetReleaseFileFacade(IConfiguration configuration) {            
            ReleaseLocalDataAccess releaseLocalDataAccess = new ReleaseLocalDataAccess(configuration);
            ReleaseFileFacade releaseFileFacade = new ReleaseFileFacade(configuration, releaseLocalDataAccess);
            return releaseFileFacade;            
        }
    }
}
