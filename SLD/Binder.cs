using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SLD.Parameters;



namespace SLD
{
    public class Binder
    {
        Element e;
        Element pe;
        string uid;
        string type;

        Document doc;

        Entity entity;
        Schema schema;


        public Binder(Element e)
        {
            this.e = e;
            this.doc = e.Document;
        }

        public void Bind(string uid)
        {
            if (uid != null)
            {
                this.uid = uid;
                this.pe = doc.GetElement(uid);
                BindElement_2_01();
            }
        }

        public void Bind(string uid, string type)
        {
            if (uid != null)
            {
                this.uid = uid;
                this.pe = doc.GetElement(uid);
                this.type = type;
                BindElement_2_01();
            }
        }

        public void Bind(Element pe)
        {
            this.uid = pe.UniqueId;
            this.pe = pe;
            BindElement_2_01();
        }

        public bool IsOnSheet()
        {
            return true;
        }


        public List<View> GetViews()
        {
            ICollection<Element> views = new FilteredElementCollector(doc)
                       .OfClass(typeof(View))
                       .WhereElementIsNotElementType()
                       .ToList();

            //Views with storage

            List<View> viewsa = new List<View>();

            foreach (Element view in views)
            {
                IList<Guid> eGuids = view.GetEntitySchemaGuids();

                if (eGuids.Contains(BIND_GUID_2_01))
                {
                    Schema schema = Schema.Lookup(BIND_GUID_2_01);
                    string uid = GetStrData(view, schema, BIND_LINKEDITEMUID);
                    string thisItemUid = GetStrData(view, schema, BIND_THISITEMUID);

                    if (thisItemUid == view.UniqueId && e.UniqueId == uid)
                    {
                        viewsa.Add(view as View);
                    }
                }
            }
            return viewsa;
        }

        public List<ElementId> GetViewIds()
        {
            ICollection<Element> views = new FilteredElementCollector(doc)
                       .OfClass(typeof(View))
                       .WhereElementIsNotElementType()
                       .ToList();

            //Views with storage

            List<ElementId> viewsa = new List<ElementId>();

            foreach (Element view in views)
            {
                IList<Guid> eGuids = view.GetEntitySchemaGuids();

                if (eGuids.Contains(BIND_GUID_2_01))
                {
                    Schema schema = Schema.Lookup(BIND_GUID_2_01);
                    string uid = GetStrData(view, schema, BIND_LINKEDITEMUID);
                    string thisItemUid = GetStrData(view, schema, BIND_THISITEMUID);

                    if (thisItemUid == view.UniqueId && e.UniqueId == uid)
                    {
                        viewsa.Add(view.Id);
                    }
                }
            }
            return viewsa;
        }

        public string GetType()
        {
            IList<Guid> eGuids = e.GetEntitySchemaGuids();

            if (!eGuids.Contains(BIND_GUID_2_01))
            {
                return null;
            }
            // проверить будет работать? 
            Schema schema = Schema.Lookup(BIND_GUID_2_01);
            return GetStrData(e, schema, BIND_TYPE);
        }

        public string LinkedItemId()
        {
            IList<Guid> eGuids = e.GetEntitySchemaGuids();

            if (!eGuids.Contains(BIND_GUID_2_01))
            {
                return null;
            }

            // проверить будет работать? 
            Schema schema = Schema.Lookup(BIND_GUID_2_01);

            return GetStrData(e, schema, BIND_LINKEDITEMUID); ;
        }

        Schema CreateSchema_2_01()
        {
            SchemaBuilder schemaBuilder = new SchemaBuilder(BIND_GUID_2_01);
            schemaBuilder.SetReadAccessLevel(AccessLevel.Public);

            FieldBuilder isValid = schemaBuilder.AddSimpleField(BIND_ISVALID, typeof(bool));
            FieldBuilder thisItemUID = schemaBuilder.AddSimpleField(BIND_THISITEMUID, typeof(string));
            FieldBuilder linkedItemUID = schemaBuilder.AddSimpleField(BIND_LINKEDITEMUID, typeof(string));
            FieldBuilder type = schemaBuilder.AddSimpleField(BIND_TYPE, typeof(string));
            FieldBuilder dateTime = schemaBuilder.AddSimpleField(BIND_DATETIME, typeof(string));

            FieldBuilder reserveDouble1 = schemaBuilder.AddSimpleField(BIND_RESERVE_DOUBLE_1, typeof(double));
            reserveDouble1.SetUnitType(UnitType.UT_Custom);

            FieldBuilder reserveDouble2 = schemaBuilder.AddSimpleField(BIND_RESERVE_DOUBLE_2, typeof(double));
            reserveDouble2.SetUnitType(UnitType.UT_Custom);

            FieldBuilder reserveDouble3 = schemaBuilder.AddSimpleField(BIND_RESERVE_DOUBLE_3, typeof(double));
            reserveDouble3.SetUnitType(UnitType.UT_Custom);

            FieldBuilder reserveDouble4 = schemaBuilder.AddSimpleField(BIND_RESERVE_DOUBLE_4, typeof(double));
            reserveDouble4.SetUnitType(UnitType.UT_Custom);

            FieldBuilder reserveDouble5 = schemaBuilder.AddSimpleField(BIND_RESERVE_DOUBLE_5, typeof(double));
            reserveDouble5.SetUnitType(UnitType.UT_Custom);

            FieldBuilder reserveInt1 = schemaBuilder.AddSimpleField(BIND_RESERVE_INT_1, typeof(int));
            FieldBuilder reserveInt2 = schemaBuilder.AddSimpleField(BIND_RESERVE_INT_2, typeof(int));
            FieldBuilder reserveInt3 = schemaBuilder.AddSimpleField(BIND_RESERVE_INT_3, typeof(int));
            FieldBuilder reserveInt4 = schemaBuilder.AddSimpleField(BIND_RESERVE_INT_4, typeof(int));
            FieldBuilder reserveInt5 = schemaBuilder.AddSimpleField(BIND_RESERVE_INT_5, typeof(int));

            FieldBuilder reserveString1 = schemaBuilder.AddSimpleField(BIND_RESERVE_STRING_1, typeof(string));
            FieldBuilder reserveString2 = schemaBuilder.AddSimpleField(BIND_RESERVE_STRING_2, typeof(string));
            FieldBuilder reserveString3 = schemaBuilder.AddSimpleField(BIND_RESERVE_STRING_3, typeof(string));
            FieldBuilder reserveString4 = schemaBuilder.AddSimpleField(BIND_RESERVE_STRING_4, typeof(string));
            FieldBuilder reserveString5 = schemaBuilder.AddSimpleField(BIND_RESERVE_STRING_5, typeof(string));

            FieldBuilder reserveBool1 = schemaBuilder.AddSimpleField(BIND_RESERVE_BOOL_1, typeof(bool));
            FieldBuilder reserveBool2 = schemaBuilder.AddSimpleField(BIND_RESERVE_BOOL_2, typeof(bool));
            FieldBuilder reserveBool3 = schemaBuilder.AddSimpleField(BIND_RESERVE_BOOL_3, typeof(bool));
            FieldBuilder reserveBool4 = schemaBuilder.AddSimpleField(BIND_RESERVE_BOOL_4, typeof(bool));
            FieldBuilder reserveBool5 = schemaBuilder.AddSimpleField(BIND_RESERVE_BOOL_5, typeof(bool));

            schemaBuilder.SetSchemaName(BIND_SCHEMANAME_2_01);
            Schema schema = schemaBuilder.Finish();

            return schema;
        }

        void BindElement_2_01()
        {
            schema = Schema.Lookup(BIND_GUID_2_01);
            if (schema == null)
            {
                try
                {
                    schema = CreateSchema_2_01();
                }
                catch
                {
                    return;
                }
            }

            //using (Transaction tr = new Transaction(doc, "Update Panel Data for Revit SLD"))
            //{
            //   tr.Start();
            entity = new Entity(schema);
            SetData(BIND_ISVALID, true);
            SetData(BIND_LINKEDITEMUID, uid);
            SetData(BIND_THISITEMUID, e.UniqueId);
            SetData(BIND_TYPE, type);
            SetData(BIND_DATETIME, DateTime.Now.ToString());
            e.SetEntity(entity);
            //tr.Commit();
            //}

        }

        public void DeleteSchema(Guid guid)
        {
            //Add transaction
            Schema schema = Schema.Lookup(guid);
            Schema.EraseSchemaAndAllEntities(schema, true);
        }

        string GetStrData(Element e, Schema schema, string fieldName)
        {
            if (schema == null) { return "-1"; }

            try
            {
                Entity retrivedEntity = e.GetEntity(schema);
                string retrivedData = retrivedEntity.Get<string>(schema.GetField(fieldName));
                return retrivedData;
            }
            catch
            {
                return "-1";
            }
        }

        void SetData(string fieldName, double value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                entity.Set<double>(fieldSpliceLocation, value, DisplayUnitType.DUT_CUSTOM);
            }
            catch
            {

            }
        }

        void SetData(string fieldName, string value)
        {
            try
            {
                if (value != null)
                {
                    Field fieldSpliceLocation = schema.GetField(fieldName);
                    entity.Set<string>(fieldSpliceLocation, value);
                }
            }
            catch
            {

            }
        }

        void SetData(string fieldName, int value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                entity.Set<int>(fieldSpliceLocation, value);
            }
            catch
            {

            }
        }

        void SetData(string fieldName, bool value)
        {
            try
            {
                Field fieldSpliceLocation = schema.GetField(fieldName);
                entity.Set<bool>(fieldSpliceLocation, value);
            }
            catch
            {

            }
        }

        public static bool IsValid(Element e)
        {
            IList<Guid> eGuids = e.GetEntitySchemaGuids();
            
            if (eGuids.Contains(STRG_GUID_2_01))
            {
                //Schema schema = Schema.Lookup(STRG_GUID_2_01);
                //Entity entity = e.GetEntity(schema);
                return true;
            }

            return false;
        }

        /*  public static View GetRefView(Element e)
          {
              IList<Guid> eGuids = e.GetEntitySchemaGuids();

              if (eGuids.Contains(STRG_GUID_2_01))
              {
                  Schema schema = Schema.Lookup(STRG_GUID_2_01);
                  Entity entity = e.GetEntity(schema);

                  //Schema schema = Schema.Lookup(STRG_GUID_2_01);
                  //Entity entity = e.GetEntity(schema);
                  return true;
              }
          }

      */

        public static bool IsRefViewExists(Document doc, string uid)
        {
            try
            {
                List<Element> viewSheets = GetViewSheet(doc, uid);
                if (viewSheets.Count() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }

            try
            {
                List<Element> draftViews = GetDraftView(doc, uid);
                if (draftViews.Count() > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public static List<Element> GetViewSheet(Document doc, string uid)
        {
            //Get uids
            
            ICollection<Element> views = new FilteredElementCollector(doc)
                       .OfClass(typeof(ViewSheet))
                       .WhereElementIsNotElementType()
                       .ToList();

            foreach (Element view in views)
            {
                Storage s = new Storage(view);
            }
            return views.ToList();
        }

        public static List<Element> GetDraftView(Document doc, string uid)
        {
            ICollection<Element> views = new FilteredElementCollector(doc)
                       .OfClass(typeof(View))
                       .WhereElementIsNotElementType()
                       .Where(e => e.UniqueId.Equals(uid)).ToList();
            return views.ToList();
        }
    }
}
