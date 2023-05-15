using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using System.Collections.ObjectModel;
using GettingSimpleInfo.Infrastructure;

namespace GettingSimpleInfo
{
    public class RevitModelForfard
    {
        private UIApplication UIApp { get; set; } = null;
        private Application App { get; set; } = null;
        private UIDocument UIDoc { get; set; } = null;
        public Document Doc { get; set; } = null;

        public RevitModelForfard(UIApplication uiapp)
        {
            UIApp = uiapp;
            App = uiapp.Application;
            UIDoc = uiapp.ActiveUIDocument;
            Doc = uiapp.ActiveUIDocument.Document;
        }

        public List<string> GetAllRooms()
        {
            var rooms = new FilteredElementCollector(Doc).OfCategory(BuiltInCategory.OST_Rooms)
                                                          .Cast<Room>()
                                                          .Select(r => r.Name)
                                                          .ToList();

            return rooms;
        }

        public string GetElementBySelection()
        {
            Selection sel = UIDoc.Selection;
            Reference picked = sel.PickObject(ObjectType.Element, "Выберете элемент в модели");
            Element elem = Doc.GetElement(picked);

            return elem.Name;
        }

        public void SetElementMark(string mark)
        {
            Selection sel = UIDoc.Selection;
            Reference picked = sel.PickObject(ObjectType.Element, "Выберете элемент в модели");
            Element elem = Doc.GetElement(picked);

            var param = elem.get_Parameter(BuiltInParameter.ALL_MODEL_MARK);
            using(Transaction trans = new Transaction(Doc, "Set Mark"))
            {
                trans.Start();
                param.Set(mark);
                trans.Commit();
            }
        }
    }
}
