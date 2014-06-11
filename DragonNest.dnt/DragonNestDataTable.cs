using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace DragonNest.ResourceInspection.Dnt
{
    public class DragonNestDataTable : DataTable
    {
        public event EventHandler StatusChanged;
        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                if (StatusChanged != null)
                    StatusChanged(this, EventArgs.Empty);
            }
        }
        int status;

        public DragonNestDataTable() : base()
        {

        }
        public DragonNestDataTable(string tableName) : base(tableName)
        {
        }

        public DragonNestDataTable(Stream stream) : base()
        {
            LoadDntStream(stream);
        }

        public DragonNestDataTable LoadDntStream(Stream stream)
        {

            Columns.Add(new DataColumn("Row Id", typeof(uint)));
            stream.Position = 4L;

            using (var reader = new BinaryReader(stream))
            {
                Status = Convert.ToInt32(decimal.Divide(stream.Position, stream.Length) * 100);
                int columnCount = reader.ReadUInt16();
                uint rowCount = reader.ReadUInt32();
                for (int i = 0; i < columnCount; i++)
                {
                    uint length = reader.ReadUInt16();
                    var name = new string(reader.ReadChars((int)length));
                    switch (reader.ReadByte())
                    {
                        case 1:
                            Columns.Add(new DataColumn(name, typeof(string)));
                            break;
                        case 2:
                            Columns.Add(new DataColumn(name, typeof(bool)));
                            break;
                        case 3:
                            Columns.Add(new DataColumn(name, typeof(int)));
                            break;
                        case 4:
                            Columns.Add(new DataColumn(name, typeof(float)));
                            break;
                        case 5:
                            Columns.Add(new DataColumn(name, typeof(double)));
                            break;
                        default:
                            throw new FormatException("stream is not in the correct format");
                    }
                }

                for (int i = 0; (ulong)i < (ulong)rowCount; i++)
                {
                    DataRow current = NewRow();
                    for (int j = 0; j <= columnCount; j++)
                    {
                        if (Columns[j].DataType == typeof(uint))
                            current[Columns[j].ColumnName] = reader.ReadUInt32();
                        if (Columns[j].DataType == typeof(string))
                            current[Columns[j].ColumnName] = Encoding.ASCII.GetString(reader.ReadBytes(reader.ReadInt16()));
                        if (Columns[j].DataType == typeof(bool))
                            current[Columns[j].ColumnName] = reader.ReadInt32();
                        if (Columns[j].DataType == typeof(int))
                            current[Columns[j].ColumnName] = reader.ReadInt32();
                        if (Columns[j].DataType == typeof(float))
                            current[Columns[j].ColumnName] = reader.ReadSingle();
                        if (Columns[j].DataType == typeof(double))
                            current[Columns[j].ColumnName] = reader.ReadSingle();
                    }
                    Rows.Add(current);
                    Status = Convert.ToInt32(decimal.Divide(stream.Position, stream.Length) * 100);
                }
            }
            return this;
        }
    }
}
