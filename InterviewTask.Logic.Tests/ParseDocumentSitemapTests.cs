﻿using InterviewTask.Logic.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace InterviewTask.Logic.Tests
{
    [TestFixture]
    internal class ParseDocumentSitemapTests
    {
        private ParseDocumentSitemap _parserDocumentSitemap;

        [SetUp]
        public void SetUp()
        {
            _parserDocumentSitemap = new ParseDocumentSitemap();
        }

        [Test]
        public void ParseDocument_BlankDocument_ReturnEmpty()
        {
            //Arrange
            string document = string.Empty;

            //Act
            IEnumerable<Uri> actual = _parserDocumentSitemap.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actual);
        }

        [Test]
        public void ParseDocument_DocumentNotHaveLinks_ReturnEmptyList()
        {
            //Arrange
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"http://www.w3.org/1999/xhtml/\"></urlset>";

            //Act
            var actualLink = _parserDocumentSitemap.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualLink);
        }

        [Test]
        public void ParseDocument_DocumentHaveSeveralLink_ReturnList()
        {
            //Arrange
            var expectedLinks = new List<Uri>(3) { new Uri("https://test1.com/tea/"), new Uri("https://test2.com/coffe/"), new Uri("https://test3.com/") };
            string document = "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xhtml=\"https://www.w3.org/1999/xhtml/\"> " +
                "<url> <loc>https://test1.com/tea/</loc> </url> <url> <loc>https://test2.com/coffe/</loc> </url> " +
                "<url> <loc>https://test3.com/</loc> </url> </urlset>";

            //Act
            var actualLinks = _parserDocumentSitemap.ParseDocument(document);

            //Assert
            Assert.AreEqual(expectedLinks, actualLinks);
        }

        [Test]
        public void ParseDocument_PassingNullIsParameters_ReturnList()
        {
            //Arrange
            string document = null;

            //Act
            var actualLinks = _parserDocumentSitemap.ParseDocument(document);

            //Assert
            Assert.IsEmpty(actualLinks);
        }
    }
}
