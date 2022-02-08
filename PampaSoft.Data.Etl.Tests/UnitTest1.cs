using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Robin.Data.ParkingETL;
using Robin.Data.ParkingETL.Actions;
using Robin.Data.ParkingETL.Destination;
using Robin.Data.ParkingETL.Format;
using Robin.Data.ParkingETL.Source;
using Action = Robin.Data.ParkingETL.Actions.Action;

namespace PampaSoft.Data.Etl.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task HttpSource()
        {
            IDataSource dataSource = new HttpDataSource("http://google.com");

            Assert.IsTrue(await dataSource.Open());
            dataSource.Close();
            
            dataSource = new HttpDataSource("http://iubfsidubfi.sudf");
            Assert.IsFalse(await dataSource.Open());
            dataSource.Close();
        }
        
        [Test]
        public void DatatableFunctions()
        {
            DataTable dataTable = new DataTable();
            Assert.Zero(dataTable.Count);
            
            dataTable.SetHeader(new []{"key"});
            bool resOk = dataTable.InsertLine(new[] {"test"});
            
            Assert.NotZero(dataTable.Count);
            Assert.IsTrue(resOk);

            dataTable.SetHeader(new []{"key", "key_2"});
            bool resNok = dataTable.InsertLine(new[] {"test"});
            
            Assert.IsFalse(resNok);
            
            Assert.IsNull(dataTable.Column("nope"));

            DataTable source = new DataTable();
            DataTable toAppend = new DataTable();
            
            source.SetHeader(new []{"Col1", "Col2", "Col3"});
            source.InsertLine(new[] {"1", "2", "3"});
            
            toAppend.SetHeader(new []{"Col1", "Col2"});
            toAppend.InsertLine(new[] {"3", "4"});
            
            source.Append(toAppend);
            Assert.AreEqual(2, source.Count);
            Assert.IsNull(source.Column("Col3").ToList()[1]);
        }

        [Test]
        public async Task TestCsvFromHttp()
        {
            IDataSource dataSource = new HttpDataSource("https://opendata.lillemetropole.fr/explore/dataset/disponibilite-parkings/download/?format=csv&timezone=Europe/Berlin&lang=fr&use_labels_for_header=true&csv_separator=%3B");
            IDataFormat dataFormat = new CsvDataFormat(true, ';', '\n');
            
            await dataSource.Open();
            ICollection<DataTable> dataTables = dataSource.ReadAll(dataFormat);

            Assert.AreEqual(1, dataTables.Count);
            Assert.NotZero(dataTables.First().Count);
        }
        
        [Test]
        public async Task TestJsonFromHttp()
        {
            IDataSource dataSource = new HttpDataSource("https://data.mobilites-m.fr/api/dyn/parking/json");
            IDataFormat dataFormat = new JsonDataFormat();
            
            await dataSource.Open();
            ICollection<DataTable> dataTables = dataSource.ReadAll(dataFormat);
            
            Assert.AreEqual(1, dataTables.Count);
            Assert.NotZero(dataTables.First().Count);
        }
        
        [Test]
        public async Task TestXmlFromHttp()
        {
            IDataSource dataSource = new HttpDataSource("https://data.montpellier3m.fr/sites/default/files/ressources/FR_MTP_ANTI.xml");
            IDataFormat dataFormat = new XmlDataFormat();
            
            await dataSource.Open();
            ICollection<DataTable> dataTables = dataSource.ReadAll(dataFormat);
            
            Assert.AreEqual(1, dataTables.Count);
            Assert.NotZero(dataTables.First().Count);
        }
        
        [Test]
        public async Task Reshape()
        {
            IDataSource dataSource = new HttpDataSource("https://data.mobilites-m.fr/api/dyn/parking/json");
            IDataFormat dataFormat = new JsonDataFormat();
            
            await dataSource.Open();
            ICollection<DataTable> dataTables = dataSource.ReadAll(dataFormat);
            
            dataTables.First().ReshapeKeep(new []{"key"});
            Assert.AreEqual(1, dataTables.First().Columns.Length);
        }
        
        [Test]
        public async Task Rename()
        {
            IDataSource dataSource = new HttpDataSource("https://data.mobilites-m.fr/api/dyn/parking/json");
            IDataFormat dataFormat = new JsonDataFormat();
            
            await dataSource.Open();
            ICollection<DataTable> dataTables = dataSource.ReadAll(dataFormat);

            var previousFirst = dataTables.First().Column("key").First();
            
            dataTables.First().RenameColumn("key", "id");
            Assert.AreEqual(previousFirst, dataTables.First().Column("id").First());
        }

        [Test]
        public async Task MinioConnection()
        {
            IDataDestination dataDestination = new MinioDataDestination("srv0.pampasoft.fr:9100;IT4DPF0YMPXN8P81U4NY;icBLG8XYrokdXauji8WtOsf3rIJS73jyCGHnRjeP;parking-realtime");
            bool res = await dataDestination.Open();
            Assert.IsTrue(res);
            dataDestination = new MinioDataDestination("srv0.pampasoft.fr:9000;IT4DPF0YMPXN8P81U4NY;icBLG8XYrokdXauji8WtOsf3rIJS73jyCGHnRjeP;parking-realtime");
            res = await dataDestination.Open();
            Assert.IsFalse(res);
            dataDestination = new MinioDataDestination("srv0.pampasoft.fr:9100;IT4DPF0YMPXN8P81U4NY;icBLG8XYrokdXauji8WtOsf3rIJS73jyCGHnRjeP;parking");
            res = await dataDestination.Open();
            Assert.IsFalse(res);
        }

        [Test]
        public async Task MinioFilePush()
        {
            IDataDestination dataDestination = new MinioDataDestination("srv0.pampasoft.fr:9100;IT4DPF0YMPXN8P81U4NY;icBLG8XYrokdXauji8WtOsf3rIJS73jyCGHnRjeP;parking-realtime");
            bool res = await dataDestination.Open();
            
            IDataSource dataSource = new HttpDataSource("https://opendata.lillemetropole.fr/explore/dataset/disponibilite-parkings/download/?format=csv&timezone=Europe/Berlin&lang=fr&use_labels_for_header=true&csv_separator=%3B");
            IDataFormat dataFormat = new CsvDataFormat(true, ';', '\n');
            
            await dataSource.Open();
            DataTable dataTable = dataSource.ReadAll(dataFormat).First();

            await dataDestination.Push(dataTable);
        }
        
        [Test]
        public async Task MinioFileGet()
        {
            IDataSource dataSource = new MinioDataSource("srv0.pampasoft.fr:9100;IT4DPF0YMPXN8P81U4NY;icBLG8XYrokdXauji8WtOsf3rIJS73jyCGHnRjeP;parking-realtime;input/");
            bool res = await dataSource.Open();
            
            IDataFormat dataFormat = new CsvDataFormat(true, ';', '\n');
            ICollection<DataTable> dataTables = dataSource.ReadAll(dataFormat);

            Assert.NotZero(dataTables.Count);
        }

        [Test]
        public async Task NpgsqlConnection()
        {
            IDataDestination destination = new PostgreDataDestination("Host=srv0.pampasoft.fr;Username=strapi;Password=strapi;Database=parking_realtime");
            bool res = await destination.Open();
            Assert.IsTrue(res);
            destination.Close();
        }
        
        [Test]
        public async Task NpgsqlPush()
        {
            IDataSource dataSource = new MinioDataSource("srv0.pampasoft.fr:9100;IT4DPF0YMPXN8P81U4NY;icBLG8XYrokdXauji8WtOsf3rIJS73jyCGHnRjeP;parking-realtime");
            await dataSource.Open();
            
            IDataFormat dataFormat = new CsvDataFormat(true, ';', '\n');
            ICollection<DataTable> dataTables = dataSource.ReadAll(dataFormat);
            
            IDataDestination destination = new PostgreDataDestination("Host=srv0.pampasoft.fr;Username=strapi;Password=strapi;Database=parking_realtime");
            bool res = await destination.Open();
            await destination.Push(dataTables.First());
            destination.Close();
        }

        [Test]
        public async Task CompleteFlow()
        {
            IDataSource dataSource = new HttpDataSource("https://opendata.lillemetropole.fr/explore/dataset/disponibilite-parkings/download/?format=csv&timezone=Europe/Berlin&lang=fr&use_labels_for_header=true&csv_separator=%3B");
            IDataFormat dataFormat = new CsvDataFormat(true, ';', '\n');

            IAction<ICollection<DataTable>> extractAction = new ExtractAction(dataSource, dataFormat);
            if (await extractAction.Execute())
            {
                DataTable result = extractAction.GetResult().First();
                ICollection<string> toKeep = new List<string>() {"Nom parking", "Places disponibles"};
                IAction<DataTable> reshapeAction = new ReshapeAction(result, toKeep, false);
                if (await reshapeAction.Execute())
                {
                    result = reshapeAction.GetResult();
                    Assert.AreEqual(2, result.Columns.Length);
                    
                    ICollection<Tuple<string, string>> toRename = new List<Tuple<string, string>>()
                    {
                        new("Nom parking", "name"),
                        new("Places disponibles", "free")
                    };
                    IAction<DataTable> renameAction = new RenameAction(result, toRename);
                    if (await renameAction.Execute())
                    {
                        result = renameAction.GetResult();
                        Assert.IsTrue((result.Columns[0] == "name" || result.Columns[1] == "name"));
                        Assert.IsTrue((result.Columns[0] == "free" || result.Columns[1] == "free"));
                        
                        IDataDestination destination = new PostgreDataDestination("Host=srv0.pampasoft.fr;Username=strapi;Password=strapi;Database=parking_realtime");
                        IAction<bool> loadAction = new LoadAction(destination, result);
                        if (await loadAction.Execute())
                        {
                            Assert.IsTrue(loadAction.GetResult());
                        }
                        else
                        {
                            Assert.Fail();
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task ActionFlowTest()
        {
            IDataSource dataSource = new HttpDataSource("https://opendata.lillemetropole.fr/explore/dataset/disponibilite-parkings/download/?format=csv&timezone=Europe/Berlin&lang=fr&use_labels_for_header=true&csv_separator=%3B");
            IDataFormat dataFormat = new CsvDataFormat(true, ';', '\n');
            IAction<ICollection<DataTable>> extractAction = new ExtractAction(dataSource, dataFormat);

            ICollection<string> toKeep = new List<string>() {"Nom parking", "Places disponibles"};
            IAction<DataTable> reshapeAction = new ReshapeAction(toKeep, false);
            
            ICollection<Tuple<string, string>> toRename = new List<Tuple<string, string>>()
            {
                new("Nom parking", "name"),
                new("Places disponibles", "free")
            };
            IAction<DataTable> renameAction = new RenameAction(toRename);
            
            IDataDestination destination = new PostgreDataDestination("Host=srv0.pampasoft.fr;Username=strapi;Password=strapi;Database=parking_realtime");
            IAction<bool> loadAction = new LoadAction(destination);

            ActionFlow actionFlow = new ActionFlow();
            
            actionFlow.AddAction((Action) extractAction);
            actionFlow.AddAction((Action) reshapeAction);
            actionFlow.AddAction((Action) renameAction);
            actionFlow.AddAction((Action) loadAction);

            await actionFlow.Execute();
        }
    }
}