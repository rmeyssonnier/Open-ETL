using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PampaSoft.Data.Etl.Api.Models;
using PampaSoft.Data.Etl.Api.Models.ActionConfigurations;
using Robin.Data.ParkingETL;
using Robin.Data.ParkingETL.Actions;
using Robin.Data.ParkingETL.Source;

namespace PampaSoft.Data.Etl.Api.Repository
{
    public class EtlCommandBuilder
    {
        private EtlFlowCommand _command;
        public ActionFlow ActionFlow = new ActionFlow();
        
        public EtlCommandBuilder(EtlFlowCommand command)
        {
            _command = command;
        }

        public void Deserialize()
        {
            ICollection<JObject> stages = _command.Stages
                .Select(s => JsonConvert.DeserializeObject<JObject>(s))
                .OrderBy(s => int.Parse(s["actionId"].ToString()))
                .ToList();

            foreach (var stage in stages)
            {
                Console.WriteLine(stage.ToString());

                switch (stage["type"].ToString())
                {
                    case "Extract HTTP":
                    {
                        ExtractHttpConfiguration configuration = JsonConvert.DeserializeObject<ExtractHttpConfiguration>(stage["configuration"].ToString());
                        ActionFlow.AddAction(configuration.ToFlowAction());
                        break;   
                    }
                    case "Rename columns":
                    {
                        TransformRenameConfiguration configuration = JsonConvert.DeserializeObject<TransformRenameConfiguration>(stage["configuration"].ToString());
                        ActionFlow.AddAction(configuration.ToFlowAction());
                        break;
                    }
                    case "Keep columns":
                    {
                        TransformKeepColumnsConfiguration configuration = JsonConvert.DeserializeObject<TransformKeepColumnsConfiguration>(stage["configuration"].ToString());
                        ActionFlow.AddAction(configuration.ToFlowAction());
                        break;
                    }
                    case "Remove columns":
                    {
                        TransformRemoveColumnsConfiguration configuration = JsonConvert.DeserializeObject<TransformRemoveColumnsConfiguration>(stage["configuration"].ToString());
                        ActionFlow.AddAction(configuration.ToFlowAction());
                        break;
                    }
                    case "Load minio":
                    {
                        LoadMinioConfiguration configuration = JsonConvert.DeserializeObject<LoadMinioConfiguration>(stage["configuration"].ToString());
                        ActionFlow.AddAction(configuration.ToFlowAction());
                        break;
                    }
                    case "Load postgres":
                    {
                        LoadPostgresConfiguration configuration = JsonConvert.DeserializeObject<LoadPostgresConfiguration>(stage["configuration"].ToString());
                        ActionFlow.AddAction(configuration.ToFlowAction());
                        break;
                    }

                    default:
                        break;
                }
            }
        }
    }
}