﻿using System;
using System.Linq;
using EPS.Annotations;
using Xunit;

namespace EPS.Web.Handlers.Tests.Unit
{
    public class StreamWriteStatusTest
    {
        [Fact]
        public void StreamWriteStatus_EnumDescriptionShortNamesDoNotOverlap()
        {
            var enumValues = Enum.GetValues(typeof(StreamWriteStatus));
            var shortNames = enumValues.OfType<StreamWriteStatus>().Select(e => e.ToShortNameString()).ToList();
            Assert.Equal(shortNames.Count, shortNames.Distinct().Count());
        }
    }
}