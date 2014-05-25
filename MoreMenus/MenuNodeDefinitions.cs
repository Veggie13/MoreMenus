using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace MoreMenus
{
    [XmlRoot("root")]
    public class MenuXml
    {
        public static MenuXml Load(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MenuXml));
                return (MenuXml)serializer.Deserialize(stream);
            }
        }

        public static void Create(string path)
        {
            var root = new MenuXml()
            {
                Items = new[] {
                    new MenuTreeDefinition()
                    {
                        Caption = "Root",
                        Children = new MenuNodeDefinition[] {
                            new MenuTreeDefinition()
                            {
                                Caption = "Menu1",
                                Children = new [] {
                                    new MenuItemDefinition()
                                    {
                                        Caption = "Item1",
                                        Action = "Action1",
                                        Arguments = new [] {
                                            new MenuItemArgument() {
                                                Name = "Arg1",
                                                Value = "A1"
                                            },
                                            new MenuItemArgument() {
                                                Name = "Arg2",
                                                Value = "A2"
                                            }
                                        }
                                    }
                                }
                            },
                            new MenuItemDefinition()
                            {
                                Caption = "Item2",
                                Action = "Action2",
                                Arguments = new [] {
                                    new MenuItemArgument() {
                                        Name = "Arg3",
                                        Value = "A3"
                                    }
                                }
                            },
                            new MenuItemDefinition()
                            {
                                Caption = "Item3",
                                Action = "Action3",
                                Arguments = new [] {
                                    new MenuItemArgument() {
                                        Name = "Arg4",
                                        Value = "A4"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            root.Save(path);
        }

        [XmlArray("items"),
         XmlArrayItem("item")]
        public MenuNodeDefinition[] Items
        {
            get;
            set;
        }

        public void Save(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MenuXml));
                serializer.Serialize(stream, this);
            }
        }
    }

    [XmlInclude(typeof(MenuTreeDefinition)),
     XmlInclude(typeof(MenuItemDefinition))]
    public abstract class MenuNodeDefinition
    {
        [XmlAttribute("caption")]
        public string Caption
        {
            get;
            set;
        }

        public interface IVisitor
        {
            void Visit(MenuTreeDefinition node);
            void Visit(MenuItemDefinition node);
        }

        public abstract void Accept(IVisitor visitor);
    }

    public class MenuTreeDefinition : MenuNodeDefinition
    {
        [XmlArray("items"),
         XmlArrayItem("item")]
        public MenuNodeDefinition[] Children
        {
            get;
            set;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class MenuItemDefinition : MenuNodeDefinition
    {
        [XmlAttribute("action")]
        public string Action
        {
            get;
            set;
        }

        [XmlArray("arguments"),
         XmlArrayItem("arg")]
        public MenuItemArgument[] Arguments
        {
            get;
            set;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class MenuItemArgument
    {
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute("value")]
        public string Value
        {
            get;
            set;
        }
    }
}
