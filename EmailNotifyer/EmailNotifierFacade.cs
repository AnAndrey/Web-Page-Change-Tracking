﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedInterfaces;

namespace EmailNotifyer
{
    public class EmailNotifierFacade
    {
        private List<string> listOfChanges = new List<string>();
        private List<string> listOfErrors = new List<string>();

        public IDataAnalyzer Analyzer { get; private set; }
        public IDataFetcher ExternalSource { get; private set; }
        public IDataStorageProvider DataStorage { get; private set; }
        
        public INotificationManager Notificator { get; private set; }

        public EmailNotifierFacade(IDataAnalyzer analyzer, 
                                   IDataFetcher externalSource, 
                                   IDataStorageProvider dataStorage, 
                                   INotificationManager notificator)
        {
            Analyzer = analyzer;
            ExternalSource = externalSource;
            DataStorage = dataStorage;
            Notificator = notificator;
            
        }

        private void Notify()
        {
            Notificator.NotifyAbout(listOfChanges);
        }

        private void Analyzer_ErrorEvent(object sender, string e)
        {
            if(!String.IsNullOrEmpty(e))
                listOfErrors.Add(e);
        }

        private void Analyzer_DetectedDifferenceEvent(object sender, string e)
        {
            if (!String.IsNullOrEmpty(e))
                listOfChanges.Add(e);
        }

        public void FindAndNotify()
        {
            EmailNotifyer rr = new EmailNotifyer();
            rr.Notify();
            return;
            Analyzer.DetectedDifferenceEvent += Analyzer_DetectedDifferenceEvent;
            Analyzer.ErrorEvent += Analyzer_ErrorEvent;

            //Get Freash data
            var receivedData = ExternalSource.GetData();

            //Get old data
            var presavedData = DataStorage.GetData();

            if (presavedData != null)
            {
                Analyzer.Analyze(receivedData, presavedData);
                Notify();

            }

            // Clear All old data
            DataStorage.CleanStorage();
            // Save new data
            DataStorage.SaveData(receivedData);
        }

    }
}