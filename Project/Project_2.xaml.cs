using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для Project_2.xaml
    /// </summary>
    public partial class Project_2 : Window
    {
        public Project_2()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void dropZoneGrid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }
        private void dropZoneGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string filePath = files[0]; // Get the full path of the dropped file
                                            // Do something with the file path, e.g., display it in a TextBox
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Document doc = null;

                try
                {
                    // Открываем документ
                    doc = wordApp.Documents.Open(filePath);
                    wordApp.Visible = false; // Скрываем приложение

                    // Проходим по всем абзацам
                    foreach (Paragraph paragraph in doc.Paragraphs)
                    {
                        // Изменяем шрифт и размер
                        paragraph.Range.Font.Name = "Times New Roman";
                        paragraph.Range.Font.Size = 14;
                        paragraph.Range.Font.Bold = 0; // Снять жирность
                        paragraph.Range.Font.Italic = 0; // Снять курсив
                        paragraph.Range.Font.Underline = WdUnderline.wdUnderlineNone; // Снять подчеркивание
                        paragraph.Range.Font.StrikeThrough = 0; // Снять зачеркивание
                        paragraph.Range.Font.Color = WdColor.wdColorBlack; // Установить черный цвет шрифта

                        // Устанавливаем отступы
                        paragraph.FirstLineIndent = wordApp.CentimetersToPoints((float)1.25);
                        paragraph.LeftIndent = wordApp.CentimetersToPoints(0); // Отступ перед абзацем
                        paragraph.RightIndent = wordApp.CentimetersToPoints(0); // Отступ после абзаца

                        // Выравнивание по ширине
                        paragraph.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;

                        // Устанавливаем межстрочный интервал
                        paragraph.LineSpacingRule = WdLineSpacing.wdLineSpace1pt5; // Межстрочный интервал 1,5
                    }

                    // Сохраняем изменения
                    doc.Save();
                    // Устанавливаем кодировку на UTF-16
                    doc.SaveAs2(filePath, WdSaveFormat.wdFormatXMLDocument, CompatibilityMode: WdCompatibilityMode.wdWord2007);
                    MessageBox.Show("конец");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
                finally
                {
                    // Закрываем документ и приложение
                    if (doc != null)
                    {
                        doc.Close();
                    }
                    wordApp.Quit();
                }
            }
        }
    }
}
