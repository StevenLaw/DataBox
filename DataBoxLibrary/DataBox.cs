﻿using DataBoxLibrary.DataModels;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataBoxLibrary
{
    /// <summary>
    /// The main container for the data.
    /// </summary>
    [DataContract]
    public class DataBox
    {
        private string _path = string.Empty;

        [DataMember]
        protected HashSet<Tag> _tagList = new HashSet<Tag>();
        [DataMember]
        protected List<Entry> _entries = new List<Entry>();

        public string Filename { get; set; }

        public string Path 
        { 
            get
            {
                return _path;
            }
            set
            {
                _path = value.Replace("/", "\\");
                if (!value.EndsWith("\\"))
                    _path += "\\";
            }
        }

        public HashSet<Tag> TagList 
        {
            get
            {
                return _tagList;
            }
        }
        public HashSet<string> Categories
        {
            get
            {
                return new HashSet<string>(_tagList.Select(x => x.Category).Distinct().Where(x => !string.IsNullOrWhiteSpace(x)));
            }
        }
        public List<Entry> Entries 
        { 
            get
            {
                return _entries;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBox"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public DataBox(string filename)
        {
            this.Filename = filename;
        }

        #region Exists

        /// <summary>
        /// Tags the exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public bool TagExists(string name, string category = "")
        {
            return TagList.Contains(new Tag(name, category));
        }

        /// <summary>
        /// Entries the exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool EntryExists(string name)
        {
            return Entries.Any(x => x.Name == name);
        }

        #endregion

        #region New

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public Tag NewTag(string name, string category = "")
        {
            if (TagExists(name, category))
                return null;
            else
            {
                Tag tag = new Tag(name, category);
                TagList.Add(tag);
                return tag;
            }
        }

        /// <summary>
        /// Creates a new link entry.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        public LinkEntry NewLinkEntry(string name, string description)
        {
            LinkEntry entry = new LinkEntry(name, description);
            Entries.Add(entry);
            return entry;
        }

        #endregion

        #region Get Tags

        /// <summary>
        /// Gets the name of the tags by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Tag[] GetTagsByName(string name)
        {
            return _tagList.Where(x => x.Name == name).ToArray();
        }

        /// <summary>
        /// Gets the name of the tags by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public Tag[] GetTagsByName(string name, string category)
        {
            return _tagList.Where(x => x.Name == name && x.Category == category).ToArray();
        }

        /// <summary>
        /// Gets the tags by category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public Tag[] GetTagsByCategory(string category)
        {
            return _tagList.Where(x => x.Category == category).ToArray();
        }

        #endregion

        #region Get Items

        /// <summary>
        /// Gets the entries by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Entry[] GetEntriesByName(string name)
        {
            return _entries.Where(x => x.Name == name).ToArray();
        }

        /// <summary>
        /// Gets the entries by tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public Entry[] GetEntriesByTag(Tag tag)
        {
            return _entries.Where(x => x.Tags.Any(y => y == tag)).ToArray();
        }

        /// <summary>
        /// Gets the entries by the name of the tag.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <returns></returns>
        public Entry[] GetEntriesByTagName(string tagName)
        {
            return _entries.Where(x => x.Tags.Any(y => y.Name == tagName)).ToArray();
        }

        /// <summary>
        /// Gets the entries by tag category.
        /// </summary>
        /// <param name="tagCategory">The tag category.</param>
        /// <returns></returns>
        public Entry[] GetEntriesByTagCategory(string tagCategory)
        {
            return _entries.Where(x => x.Tags.Any(y => y.Category == tagCategory)).ToArray();
        }

        public LinkEntry[] GetEntriesByLinkTag(Tag linkTag)
        {
            return (from e in _entries
                    where e is LinkEntry &&
                    ((LinkEntry)e).Links.Any(x => x.Tags.Any(y => y == linkTag))
                    select (LinkEntry)e).ToArray();
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns></returns>
        public String Serialize()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(DataBox));
                XmlDictionaryWriter writer = XmlDictionaryWriter.CreateDictionaryWriter(XmlWriter.Create(stream));
                ser.WriteObject(writer, this, new Utilities.DataboxResolver());
                writer.Flush();

                stream.Position = 0;
                using (StreamReader sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (ZipFile zip = new ZipFile())
            {
                if (Path.EndsWith("/") || Path.EndsWith("\\"))
                    Path += "\\";
                zip.AddEntry("Data.xml", Serialize());
                zip.Save(Path + Filename);
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="password">The password.</param>
        public void Save(string password)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.Password = password;
                zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                zip.AddEntry("Data.xml", Serialize());
                zip.AddEntry("Encrypted", "");
                zip.Save(Path + Filename);
            }
        }

        /// <summary>
        /// Deserializes the specified serialized.
        /// </summary>
        /// <param name="serialized">The serialized.</param>
        /// <returns></returns>
        public static DataBox Deserialize(string serialized)
        {
            using (StringReader tr = new StringReader(serialized))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(DataBox));
                return (DataBox)serializer.ReadObject(XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(tr)), false, new Utilities.DataboxResolver());
            }
        }

        /// <summary>
        /// Determines whether the specified filename is encrypted.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static bool IsEncrypted(string filename)
        {
            using (ZipFile zip = ZipFile.Read(filename))
            {
                return zip.EntryFileNames.Contains("Encrypted");
            }
        }

        /// <summary>
        /// Opens the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static DataBox Open(string filename)
        {
            using (ZipFile zip = ZipFile.Read(filename))
            {
                if (zip.EntryFileNames.Contains("Data.xml"))
                {
                    var entry = zip["Data.xml"];
                    using (var ms = new MemoryStream())
                    {
                        entry.Extract(ms);

                        ms.Position = 0;
                        var sr = new StreamReader(ms);
                        DataBox db = Deserialize(sr.ReadToEnd());
                        db.Path = System.IO.Path.GetDirectoryName(filename);
                        db.Filename = System.IO.Path.GetFileName(filename);
                        return db;
                    }
                }
                else return null;
            }
        }

        /// <summary>
        /// Opens the specified filename.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="IncorrectPasswordException"/>
        public static DataBox Open(string filename, string password)
        {
            using (ZipFile zip = ZipFile.Read(filename))
            {
                if (zip.EntryFileNames.Contains("Data.xml"))
                {
                    var entry = zip["Data.xml"];
                    using (var ms = new MemoryStream())
                    {
                        try
                        {
                            entry.ExtractWithPassword(ms, password);
                        } catch (BadPasswordException e)
                        {
                            throw new IncorrectPasswordException(e.Message);
                        }

                        ms.Position = 0;
                        var sr = new StreamReader(ms);
                        DataBox db = Deserialize(sr.ReadToEnd());
                        db.Path = System.IO.Path.GetDirectoryName(filename);
                        db.Filename = System.IO.Path.GetFileName(filename);
                        return db;
                    }
                }
                else return null;
            }
        }

        #endregion
    }

        /// <summary>
        /// An <see cref="Exception"/> to indicate an incorrect password without forcing the parent to load DotNetZip.
        /// </summary>
        public class IncorrectPasswordException : Exception
        {
            public IncorrectPasswordException() { }

            public IncorrectPasswordException(string message) : base(message) { }

            public IncorrectPasswordException(string message, Exception inner) : base(message, inner) { }
        }
}
