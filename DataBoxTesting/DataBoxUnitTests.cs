using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataBoxLibrary;
using System.Linq;
using DataBoxLibrary.DataModels;

namespace DataBoxTesting
{
    [TestClass]
    public class DataBoxUnitTests
    {
        [TestMethod]
        public void DataBoxFilenameIsSaved()
        {
            //Arrange
            
            //Act
            var data = new DataBox("test.xml");

            //Assert
            Assert.AreEqual("test.xml", data.Filename);
        }

        [TestMethod]
        public void DataBoxAddTag()
        {
            //Arrange
            var data = new DataBox("test.xml");

            //Act
            var tag = data.NewTag("Test");

            //Assert
            Assert.AreEqual(1, data.TagList.Count);
            Assert.AreEqual("Test", data.TagList.First().Name);
            Assert.AreEqual(tag, data.TagList.First());
        }

        //[TestMethod]
        //public void DataBoxAddComplexTag()
        //{
        //    //Arrange
        //    var data = new DataBox("test.xml");

        //    //Act
        //    var tag = data.NewTag("test", "category", "subcategory");

        //    //Assert
        //    Assert.AreEqual("test", data.TagList.First().Name);
        //    Assert.AreEqual("category", data.TagList.First().Category);
        //    Assert.AreEqual("subcategory", data.TagList.First().Subcategory);
        //    Assert.AreEqual(tag, data.TagList.First());
        //}

        [TestMethod]
        public void DataBoxAddMultiTags()
        {
            //Arrange
            var data = new DataBox("test.xml");

            //Act
            data.NewTag("test 1");
            data.NewTag("test 2");
            data.NewTag("test 3");

            //Assert
            Assert.AreEqual(3, data.TagList.Count);
        }

        [TestMethod]
        public void DataBoxAddItem()
        {
            //Arrange
            var data = new DataBox("test.xml");

            //Act
            LinkEntry entry = data.NewLinkEntry("name", "description");

            //Assert
            Assert.AreEqual(1, data.Entries.Count);
            Assert.AreEqual("name", data.Entries[0].Name);
            Assert.AreEqual("description", ((LinkEntry)data.Entries[0]).Description);
            Assert.AreEqual(entry, data.Entries[0]);
        }

        [TestMethod]
        public void DataBoxAddMultiItems()
        {
            //Arrange
            var data = new DataBox("test.xml");

            //Act
            LinkEntry entry1 = data.NewLinkEntry("name 1", "description");
            LinkEntry entry2 = data.NewLinkEntry("name 2", "description");
            LinkEntry entry3 = data.NewLinkEntry("name 3", "description");

            //Assert
            Assert.AreEqual(3, data.Entries.Count);
            Assert.AreEqual(entry1, data.Entries[0]);
            Assert.AreEqual(entry2, data.Entries[1]);
            Assert.AreEqual(entry3, data.Entries[2]);
        }

        [TestMethod]
        public void DataBoxAddItemWithTag()
        {
            //Arrange
            var data = new DataBox("test.xml");
            var tag = data.NewTag("Test");

            //Act
            LinkEntry entry = data.NewLinkEntry("name", "description");
            entry.AddTag(tag);

            //Assert
            Assert.AreEqual(1, data.Entries.Count);
            Assert.AreEqual("name", data.Entries[0].Name);
            Assert.AreEqual("description", ((LinkEntry)data.Entries[0]).Description);
            Assert.AreEqual("Test", data.Entries[0].Tags.First().Name);
            Assert.AreEqual(entry, data.Entries[0]);
            Assert.IsTrue(data.Entries[0].Tags.Contains(tag));
        }

        [TestMethod]
        public void DataBoxAddItemWithLink()
        {
            //Arrange
            var data = new DataBox("test.xml");

            //Act
            LinkEntry entry = data.NewLinkEntry("name", "description");
            LinkItem link = entry.AddLink("link", "http://testlink.ca");

            //Assert
            Assert.AreEqual(1, data.Entries.Count);
            Assert.AreEqual("link", ((LinkEntry)data.Entries[0]).Links[0].Name);
            Assert.AreEqual("http://testlink.ca", ((LinkEntry)data.Entries[0]).Links[0].Link);
            Assert.AreEqual(link, ((LinkEntry)data.Entries[0]).Links[0]);
        }

        [TestMethod]
        public void DataBoxAddItemWithLinkWithTag()
        {
            //Arrange
            var data = new DataBox("test.xml");
            var tag = data.NewTag("test", "link");

            //Act
            LinkEntry entry = data.NewLinkEntry("name", "description");
            LinkItem link = entry.AddLink("link", "http://testlink.ca");
            link.AddTag(tag);

            //Assert
            Assert.AreEqual(1, data.Entries.Count);
            Assert.AreEqual("link", ((LinkEntry)data.Entries[0]).Links[0].Name);
            Assert.AreEqual("http://testlink.ca", ((LinkEntry)data.Entries[0]).Links[0].Link);
            Assert.IsTrue(((LinkEntry)data.Entries[0]).Links[0].Tags.Any(x => x.Name == "test"));
            Assert.AreEqual(link, ((LinkEntry)data.Entries[0]).Links[0]);
            Assert.IsTrue(((LinkEntry)data.Entries[0]).Links[0].Tags.Contains(tag));
        }

        [TestMethod]
        public void DataBoxSerializeDeserialize()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var tag = data.NewTag("tag");
            var linkTag = data.NewTag("link tag", "link");
            var entry = data.NewLinkEntry("name", "description");
            var link = entry.AddLink("link", "testlink.ca");
            link.AddTag(linkTag);

            //Act
            string ser = data.Serialize();
            var deSer = DataBox.Deserialize(ser);

            //Assert
            Assert.AreEqual(data.Entries.Count, deSer.Entries.Count);
            Assert.AreEqual(data.Entries[0].Name, deSer.Entries[0].Name);
            Assert.AreEqual(((LinkEntry)data.Entries[0]).Description, ((LinkEntry)deSer.Entries[0]).Description);
            Assert.AreEqual(data.Entries[0].Tags.Count, deSer.Entries[0].Tags.Count);
            Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Name, ((LinkEntry)deSer.Entries[0]).Links[0].Name);
            Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Link, ((LinkEntry)deSer.Entries[0]).Links[0].Link);
            Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Tags.Count, ((LinkEntry)deSer.Entries[0]).Links[0].Tags.Count);
        }

        [TestMethod]
        public void DataBoxSaveOpen()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var tag = data.NewTag("tag");
            var linkTag = data.NewTag("link tag", "link");
            var entry = data.NewLinkEntry("name", "description");
            entry.AddTag(tag);
            var link = entry.AddLink("link", "testlink.ca");
            link.AddTag(linkTag);

            //Act
            data.Save();
           var deSer = DataBox.Open("test.dat");

           //Assert
           Assert.AreEqual(data.Entries.Count, deSer.Entries.Count);
           Assert.AreEqual(data.Entries[0].Name, deSer.Entries[0].Name);
           Assert.AreEqual(((LinkEntry)data.Entries[0]).Description, ((LinkEntry)deSer.Entries[0]).Description);
           Assert.AreEqual(data.Entries[0].Tags.Count, deSer.Entries[0].Tags.Count);
           Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Name, ((LinkEntry)deSer.Entries[0]).Links[0].Name);
           Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Link, ((LinkEntry)deSer.Entries[0]).Links[0].Link);
           Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Tags.Count, ((LinkEntry)deSer.Entries[0]).Links[0].Tags.Count);
        }

        [TestMethod]
        public void DataBoxSaveOpenEncrypted()
        {
            //Arrange
            var data = new DataBox("encrypt.dat");
            var tag = data.NewTag("tag");
            var linkTag = data.NewTag("link tag", "link");
            var entry = data.NewLinkEntry("name", "description");
            entry.AddTag(tag);
            var link = entry.AddLink("link", "testlink.ca");
            link.AddTag(linkTag);

            //Act
            data.Save("pass");
            var deSer = DataBox.Open("encrypt.dat", "pass");

            //Assert
            Assert.AreEqual(data.Entries.Count, deSer.Entries.Count);
            Assert.AreEqual(data.Entries[0].Name, deSer.Entries[0].Name);
            Assert.AreEqual(((LinkEntry)data.Entries[0]).Description, ((LinkEntry)deSer.Entries[0]).Description);
            Assert.AreEqual(data.Entries[0].Tags.Count, deSer.Entries[0].Tags.Count);
            Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Name, ((LinkEntry)deSer.Entries[0]).Links[0].Name);
            Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Link, ((LinkEntry)deSer.Entries[0]).Links[0].Link);
            Assert.AreEqual(((LinkEntry)data.Entries[0]).Links[0].Tags.Count, ((LinkEntry)deSer.Entries[0]).Links[0].Tags.Count);
        }

        [TestMethod]
        public void DataBoxGetTagsByName()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var tag = data.NewTag("tag");

            //Act
            Tag[] tags = data.GetTagsByName("tag");

            //Assert
            Assert.AreEqual(tag, tags[0]);
        }

        [TestMethod]
        public void DataBoxGetTagsByNameMulti()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var tag = data.NewTag("tag");
            var tag2 = data.NewTag("tag", "subTag");

            //Act
            Tag[] tags = data.GetTagsByName("tag");

            //Assert
            Assert.IsTrue(tags.Any(x => x == tag));
            Assert.IsTrue(tags.Any(x => x == tag2));
        }

        [TestMethod]
        public void DataBoxGetTagsByCategory()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var tag = data.NewTag("tag", "category");

            //Act
            Tag[] tags = data.GetTagsByCategory("category");

            //Assert
            Assert.AreEqual(tag, tags[0]);
        }

        [TestMethod]
        public void DataBoxGetTagsByCategoryMulti()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var tag = data.NewTag("tag 1", "category");
            var tag2 = data.NewTag("tag 2", "category");

            //Act
            Tag[] tags = data.GetTagsByCategory("category");

            //Assert
            Assert.IsTrue(tags.Any(x => x == tag));
            Assert.IsTrue(tags.Any(x => x == tag2));
        }

        //[TestMethod]
        //public void DataBoxGetTagsBySubsategory()
        //{
        //    //Arrange
        //    var data = new DataBox("test.dat");
        //    var tag = data.NewTag("tag", "category", "subcategory");

        //    //Act
        //    Tag[] tags = data.GetTagsBySubcategory("category", "subcategory");

        //    //Assert
        //    Assert.AreEqual(tag, tags[0]);
        //}

        //[TestMethod]
        //public void DataBoxGetTagsBySubcategoryMulti()
        //{
        //    //Arrange
        //    var data = new DataBox("test.dat");
        //    var tag = data.NewTag("tag 1", "category", "subcategory");
        //    var tag2 = data.NewTag("tag 2", "category", "subcategory");

        //    //Act
        //    Tag[] tags = data.GetTagsBySubcategory("category", "subcategory");

        //    //Assert
        //    Assert.IsTrue(tags.Any(x => x == tag));
        //    Assert.IsTrue(tags.Any(x => x == tag2));
        //}

        [TestMethod]
        public void DataBoxGetEntries()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var entry = data.NewLinkEntry("entry", "description");

            //Act
            var entries = data.GetEntriesByName("entry");
            
            //Assert
            Assert.AreEqual(entry, entries[0]);
        }

        [TestMethod]
        public void DataBoxGetEntriesByTag()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var entry = data.NewLinkEntry("entry", "description");
            entry.AddTag(data.NewTag("tag"));

            //Act
            var entries = data.GetEntriesByTag(data.GetTagsByName("tag")[0]);

            //Assert
            Assert.AreEqual(entry, entries[0]);
        }

        [TestMethod]
        public void DataBoxGetEntriesByLinkTag()
        {
            //Arrange
            var data = new DataBox("test.dat");
            var entry = data.NewLinkEntry("entry", "description");
            entry.AddLink("link", "test.ca").AddTag(data.NewTag("tag", "link"));

            //Act
            var entries = data.GetEntriesByLinkTag(data.GetTagsByName("tag", "link")[0]);

            //Assert
            Assert.AreEqual(entry, entries[0]);
        }
    }
}
