using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MyControlLibrary
{
    /// <summary>
    /// Interaction logic for PathSelector.xaml
    /// </summary>
    public partial class PathSelector : UserControl
    {
        public PathSelector()
        {
            InitializeComponent();
        }


        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register("FileName", typeof(string), typeof(PathSelector), new PropertyMetadata(string.Empty, FileNamePropertyChanged));

        private static void FileNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not null && d is PathSelector ps && e.NewValue is string newFileName && ps.tb_path.Text != newFileName)
            {
                if (!ps.IsFolderPicker)
                {
                    ps.tb_path.Text = newFileName;
                }
            }
        }

        public string FileFiterSeperator
        {
            get { return (string)GetValue(FileFiterSeperatorProperty); }
            set { SetValue(FileFiterSeperatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileFiterSeperator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileFiterSeperatorProperty =
            DependencyProperty.Register("FileFiterSeperator", typeof(string), typeof(PathSelector), new PropertyMetadata(";"));


        public string FileFilterNameTypeSperator
        {
            get { return (string)GetValue(FileFilterNameTypeSperatorProperty); }
            set { SetValue(FileFilterNameTypeSperatorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileFilterNameTypeSperator.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileFilterNameTypeSperatorProperty =
            DependencyProperty.Register("FileFilterNameTypeSperator", typeof(string), typeof(PathSelector), new PropertyMetadata(","));



        public string FileFilters
        {
            get { return (string)GetValue(FileFiltersProperty); }
            set { SetValue(FileFiltersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileFilters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileFiltersProperty =
            DependencyProperty.Register("FileFilters", typeof(string), typeof(PathSelector), new PropertyMetadata(string.Empty));

        public bool IsFolderPicker
        {
            get { return (bool)GetValue(IsFolderPickerProperty); }
            set { SetValue(IsFolderPickerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsFolderPicker.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsFolderPickerProperty =
            DependencyProperty.Register("IsFolderPicker", typeof(bool), typeof(PathSelector), new PropertyMetadata(false));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(PathSelector),
                new PropertyMetadata(string.Empty, PathPropertyChanged));

        private static void PathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not null && d is PathSelector ps && e.NewValue is string newPath && ps.tb_path.Text != newPath)
            {
                if (ps.IsFolderPicker)
                {
                    ps.tb_path.Text = newPath;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new()
            {
                //InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent),
                IsFolderPicker = IsFolderPicker,
                Multiselect = false,
            };
            if (!IsFolderPicker && !string.IsNullOrEmpty(FileFilters))
            {
                var filters = FileFilters.Split(FileFiterSeperator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var filter in filters)
                {
                    string[] filterInfo = filter.Split(FileFilterNameTypeSperator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    if (filterInfo.Length == 0) break;
                    var extensions = new StringBuilder();
                    for (var i = 1; i < filterInfo.Length; i++)
                    {
                        var seperator = i > 1 ? ";" : string.Empty;
                        extensions.Append(seperator);

                        if (!filterInfo[i].StartsWith("*."))
                            extensions.Append(filterInfo[i]);
                        else if (filterInfo[i].StartsWith("."))
                            extensions.Append("*" + filterInfo[i]);
                        else if (filterInfo[i].Contains("."))
                            extensions.Append(filterInfo[i]);
                        else
                            extensions.Append("*." + filterInfo[i]);
                    }

                    dialog.Filters.Add(new CommonFileDialogFilter(filterInfo[0], extensions.ToString()));
                }
            }
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (!IsFolderPicker)
                {
                    FileName = new FileInfo(dialog.FileName).Name;
                }
                Path = dialog.FileName;
            }
        }
    }
}
