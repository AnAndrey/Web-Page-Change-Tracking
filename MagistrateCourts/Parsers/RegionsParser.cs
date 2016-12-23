﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoCompany.Interfaces;
using log4net;
using System.Reflection;
using HtmlAgilityPack;
using NoCompany.Data.Properties;

namespace NoCompany.Data.Parsers
{
    public class RegionsParser : DataParserHandlerBase
    {
        public static ILog logger = LogManager.GetLogger(typeof(RegionsParser));

        public RegionsParser() : base(new DistrictsParser(), new FailureHandler())
        {
        }

        protected override List<IChangeableData> TryParce(string pageUrl)
        {
            logger.Debug(MethodBase.GetCurrentMethod().Name);

            const string magistratCourtsNodeXPath = "//a[@href and @title='Участки мировых судей']";
            const string href = "href";
            const string option = "option";

            KeepTracking(Resources.Trace_AllCurtRegionsLoad);


            var list = new List<string>();
            HtmlDocument doc = LoadHtmlDocument(pageUrl, Encoding.UTF8);
            if (doc == null)
            {
                throw new HtmlRoutineException(Resources.Error_FailedToLoadMainPage);
            }

            var magistratCourtsLinkNode = doc.DocumentNode.SelectSingleNode(magistratCourtsNodeXPath);
            if (magistratCourtsLinkNode == null)
            {
                throw new HtmlRoutineException(Resources.Error_NodeIsNotFound, magistratCourtsNodeXPath);
            }

            string magistrateCourtsLink = magistratCourtsLinkNode.Attributes[href].Value;

            if (String.IsNullOrEmpty(magistrateCourtsLink))
            {
                throw new HtmlRoutineException(Resources.Error_AttributeIsNotFound, href, magistratCourtsNodeXPath);
            }

            HtmlNode.ElementsFlags.Remove(option);
            HtmlDocument allCourtRegionsDoc = LoadHtmlDocument(pageUrl + magistrateCourtsLink, Encoding.UTF8);
            if (allCourtRegionsDoc == null)
            {
                throw new HtmlRoutineException(Resources.Error_FailedToLoadRegions);
            }

            var allCourtRegionsNode = allCourtRegionsDoc.DocumentNode.SelectSingleNode("//select[@id='ms_subj']");

            return allCourtRegionsNode.Descendants(option).Skip(1)
                    .Select(n => new CourtRegion(n.InnerText, n.Attributes["value"].Value))
                    .Cast<IChangeableData>().ToList();
        }

        private HtmlDocument LoadHtmlDocument(string v, Encoding uTF8)
        {
            throw new NotImplementedException();
        }

        private void KeepTracking(object trace_AllCurtRegionsLoad)
        {
            throw new NotImplementedException();
        }
    }
}
