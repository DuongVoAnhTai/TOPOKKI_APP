using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TOPOKKI_APP.Models.Entities;

namespace TOPOKKI_APP.Helpers
{
    internal class ExportFile
    {
        private readonly string solutionDirectory;
        private readonly string assetsDirectory;
        private readonly string outputFileDirectory;
        private readonly string fileTemplateBill;

        public ExportFile()
        {
            // Đường dẫn thư mục chính của dự án
            solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            assetsDirectory = Path.Combine(solutionDirectory, "Assets");
            outputFileDirectory = Path.Combine(solutionDirectory, "OutputFile");
            fileTemplateBill = Path.Combine(assetsDirectory, "Template", "Bill.docx");

            // Đảm bảo thư mục OutputFile tồn tại
            if (!Directory.Exists(outputFileDirectory))
            {
                Directory.CreateDirectory(outputFileDirectory);
            }
        }

        public void ExportBill(Order order)
        {
            try
            {
                // Load file template
                Document doc = new Document();
                doc.LoadFromFile(fileTemplateBill);

                using (var context = new TopokkiEntities())
                {
                    var query = (from od in context.OrderDetails
                                 join o in context.Orders on od.OrderID equals o.ID
                                 join t in context.TableFoods on o.TableID equals t.ID
                                 join p in context.Products on od.ProductID equals p.ID
                                 where od.OrderID == order.ID
                                 select new
                                 {
                                     Name = p.Name,
                                     Price = p.Price,
                                     CheckIn = o.DateCheckIn,
                                     CheckOut = o.DateCheckOut,
                                     Quantity = od.Quantity,
                                     TableName = t.Name,
                                     TotalPrice = o.TotalPrice
                                 }).ToList();


                    // Thay thế nội dung trong file template
                    doc.Replace("{EmployeeName}", User.Name, false, true);
                    doc.Replace("{TableName}", query.First().TableName, false, true);
                    doc.Replace("{CheckIn}", query.First().CheckIn.ToString("dd/MM/yyyy HH:mm"), false, true);
                    doc.Replace("{CheckOut}", query.First().CheckOut?.ToString("dd/MM/yyyy HH:mm"), false, true);



                    // Tìm vị trí của placeholder {OrderDetails}
                    TextSelection selection = doc.FindString("{OrderDetails}", true, true);
                    TextRange range = selection.GetAsOneRange();
                    Section section = range.OwnerParagraph.OwnerTextBody.Owner as Section;
                    int index = section.Body.ChildObjects.IndexOf(range.OwnerParagraph);

                    // Tạo bảng OrderDetails
                    Spire.Doc.Table tableOrderDetails = section.AddTable(true);
                    tableOrderDetails.ResetCells(query.Count() + 1, 3); // Số hàng và cột
                    tableOrderDetails.TableFormat.HorizontalAlignment = RowAlignment.Center;

                    // Định nghĩa tiêu đề bảng
                    string[] headers = { "Tên", "Số lượng", "Đơn giá" };
                    for (int col = 0; col < headers.Length; col++)
                    {
                        Paragraph headerPara = tableOrderDetails.Rows[0].Cells[col].AddParagraph();
                        TextRange headerText = headerPara.AppendText(headers[col]);
                        headerText.CharacterFormat.FontName = "Times New Roman";
                        headerText.CharacterFormat.FontSize = 14;
                        headerText.CharacterFormat.Bold = true;
                        headerPara.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;

                        // Định dạng ô
                        tableOrderDetails.Rows[0].Cells[col].CellFormat.VerticalAlignment = Spire.Doc.Documents.VerticalAlignment.Middle;
                        tableOrderDetails.Rows[0].Cells[col].CellFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.Single;
                        tableOrderDetails.Rows[0].Cells[col].CellFormat.Borders.Color = Color.Gray;
                    }

                    // Điền dữ liệu vào bảng
                    for (int i = 0; i < query.Count; i++)
                    {
                        var item = query[i];

                        // Cột 1: Tên
                        Paragraph cellPara0 = tableOrderDetails.Rows[i + 1].Cells[0].AddParagraph();
                        TextRange cellText0 = cellPara0.AppendText(item.Name);
                        cellText0.CharacterFormat.FontName = "Times New Roman";
                        cellText0.CharacterFormat.FontSize = 12;
                        tableOrderDetails.Rows[i + 1].Cells[0].CellFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.Single;
                        tableOrderDetails.Rows[i + 1].Cells[0].CellFormat.Borders.Color = Color.Gray;

                        // Cột 2: Số lượng
                        Paragraph cellPara1 = tableOrderDetails.Rows[i + 1].Cells[1].AddParagraph();
                        TextRange cellText1 = cellPara1.AppendText(item.Quantity.ToString());
                        cellText1.CharacterFormat.FontName = "Times New Roman";
                        cellText1.CharacterFormat.FontSize = 12;
                        cellPara1.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                        tableOrderDetails.Rows[i + 1].Cells[1].CellFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.Single;
                        tableOrderDetails.Rows[i + 1].Cells[1].CellFormat.Borders.Color = Color.Gray;

                        // Cột 3: Đơn giá
                        Paragraph cellPara2 = tableOrderDetails.Rows[i + 1].Cells[2].AddParagraph();
                        TextRange cellText2 = cellPara2.AppendText($"{item.Price * item.Quantity:N0} VND");
                        cellText2.CharacterFormat.FontName = "Times New Roman";
                        cellText2.CharacterFormat.FontSize = 12;
                        cellPara2.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                        tableOrderDetails.Rows[i + 1].Cells[2].CellFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.Single;
                        tableOrderDetails.Rows[i + 1].Cells[2].CellFormat.Borders.Color = Color.Gray;
                    }

                    // Thêm bảng vào vị trí của placeholder
                    section.Body.ChildObjects.Insert(index + 1, tableOrderDetails);

                    // Xóa đoạn văn chứa placeholder
                    section.Body.ChildObjects.RemoveAt(index);



                    // Tìm vị trí của {Information}
                    TextSelection selection2 = doc.FindString("{Information}", true, true);
                    TextRange range2 = selection2.GetAsOneRange();
                    Section section2 = range2.OwnerParagraph.OwnerTextBody.Owner as Section;
                    int index2 = section2.Body.ChildObjects.IndexOf(range2.OwnerParagraph);

                    // Tạo bảng mới
                    Spire.Doc.Table informationTable = section2.AddTable(true);
                    informationTable.ResetCells(1, 2); // Chỉ 1 dòng (Thành tiền) và 2 cột
                    informationTable.TableFormat.HorizontalAlignment = RowAlignment.Center;

                    // Thêm dữ liệu vào bảng
                    Paragraph para = informationTable.Rows[0].Cells[0].AddParagraph();
                    TextRange totalBillText = para.AppendText("Thành tiền");
                    totalBillText.CharacterFormat.FontName = "Times New Roman";
                    totalBillText.CharacterFormat.FontSize = 14;
                    totalBillText.CharacterFormat.Bold = true;
                    para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;
                    informationTable.Rows[0].Cells[0].CellFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.Single;
                    informationTable.Rows[0].Cells[0].CellFormat.Borders.Color = Color.White;

                    para = informationTable.Rows[0].Cells[1].AddParagraph();
                    TextRange totalBillText2 = para.AppendText(query.First().TotalPrice.ToString());
                    totalBillText2.CharacterFormat.FontName = "Times New Roman";
                    totalBillText2.CharacterFormat.FontSize = 14;
                    totalBillText2.CharacterFormat.Bold = true;
                    para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                    informationTable.Rows[0].Cells[1].CellFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.Single;
                    informationTable.Rows[0].Cells[1].CellFormat.Borders.Color = Color.White;

                    // Chèn bảng vào vị trí của {Information}
                    section2.Body.ChildObjects.Insert(index2 + 1, informationTable);
                    section2.Body.ChildObjects.Remove(range2.OwnerParagraph);


                    // Đường dẫn file PDF đầu ra
                    string outputFilePath = Path.Combine(outputFileDirectory, "Bill_" + order?.ID + ".pdf");


                    // Lưu file dưới dạng PDF
                    doc.SaveToFile(outputFilePath, FileFormat.PDF);

                    MessageBox.Show("Hóa đơn đã xuất thành công");
                    //return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //return false;
            }
        }
    }
}
