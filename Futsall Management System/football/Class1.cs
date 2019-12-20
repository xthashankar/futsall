﻿using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace football
{
    #region Supporting Classes

    /// <summary>
    /// Class for the ownerdraw event. Provide the caller with the cell data, the current
    /// graphics context and the location in which to draw the cell.
    /// </summary>
    public class DGVCellDrawingEventArgs : EventArgs
    {
        public Graphics g;
        public RectangleF DrawingBounds;
        public DataGridViewCellStyle CellStyle;
        public int row;
        public int column;
        public Boolean Handled;

        public DGVCellDrawingEventArgs(Graphics g, RectangleF bounds, DataGridViewCellStyle style,
            int row, int column)
            : base()
        {
            this.g = g;
            DrawingBounds = bounds;
            CellStyle = style;
            this.row = row;
            this.column = column;
            Handled = false;
        }
    }

    /// <summary>
    /// Delegate for ownerdraw cells - allow the caller to provide drawing for the cell
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CellOwnerDrawEventHandler(object sender, DGVCellDrawingEventArgs e);

    /// <summary>
    /// Hold Extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Extension method to print all the "ImbeddedImages" in a provided list
        /// </summary>
        /// <typeparam name="?"></typeparam>
        /// <param name="list"></param>
        /// <param name="g"></param>
        /// <param name="pagewidth"></param>
        /// <param name="pageheight"></param>
        /// <param name="margins"></param>
        public static void DrawImbeddedImage<T>(this IEnumerable<T> list,
            Graphics g, int pagewidth, int pageheight, Margins margins)
        {
            foreach (T t in list)
            {
                if (t is DGVPrinter.ImbeddedImage)
                {
                    DGVPrinter.ImbeddedImage ii = (DGVPrinter.ImbeddedImage)Convert.ChangeType(t, typeof(DGVPrinter.ImbeddedImage));
                    // Fix - DrawImageUnscaled was actually scaling the images!!?! Oh well...
                    //g.DrawImageUnscaled(ii.theImage, ii.upperleft(pagewidth, pageheight, margins));
                    g.DrawImage(ii.theImage,
                        new Rectangle(ii.upperleft(pagewidth, pageheight, margins),
                        new Size(ii.theImage.Width, ii.theImage.Height)));
                }
            }
        }

    }

    #endregion

    /// <summary>
    /// Data Grid View Printer. Print functions for a datagridview, since MS
    /// didn't see fit to do it.
    /// </summary>
    public class DGVPrinter
    {
        public enum Alignment { NotSet, Left, Right, Center }
        public enum Location { Header, Footer, Absolute }
        public enum SizeType { CellSize, StringSize, Porportional }
        public enum PrintLocation { All, FirstOnly }

        //---------------------------------------------------------------------
        // internal classes/structs
        //---------------------------------------------------------------------

        // Allow the user to provide images that will be printed as either logos in the
        // header and/or footer or watermarked as in printed behind the text.
        public class ImbeddedImage
        {
            public Image theImage { get; set; }
            public Alignment ImageAlignment { get; set; }
            public Location ImageLocation { get; set; }
            public Int32 ImageX { get; set; }
            public Int32 ImageY { get; set; }

            internal Point upperleft(int pagewidth, int pageheight, Margins margins)
            {
                int y = 0;
                int x = 0;

                // if we've been given an absolute location, just use it
                if (ImageLocation == Location.Absolute)
                    return new Point(ImageX, ImageY);

                // set the y location based on header or footer
                switch (ImageLocation)
                {
                    case Location.Header:
                        y = margins.Top;
                        break;
                    case Location.Footer:
                        y = pageheight - theImage.Height - margins.Bottom;
                        break;
                    default:
                        throw new ArgumentException(String.Format("Unkown value: {0}", ImageLocation));
                }

                // set the x location based on left,right,center
                switch (ImageAlignment)
                {
                    case Alignment.Left:
                        x = margins.Left;
                        break;
                    case Alignment.Center:
                        x = (int)(pagewidth / 2 - theImage.Width / 2) + margins.Left;
                        break;
                    case Alignment.Right:
                        x = (int)(pagewidth - theImage.Width) + margins.Left;
                        break;
                    case Alignment.NotSet:
                        x = ImageX;
                        break;
                    default:
                        throw new ArgumentException(String.Format("Unkown value: {0}", ImageAlignment));
                }

                return new Point(x, y);
            }
        }

        public IList<ImbeddedImage> ImbeddedImageList = new List<ImbeddedImage>();

        // handle wide-column printing - that is, lists of columns that extend
        // wider than one page width. Columns are broken up into "Page Sets" that
        // are printed one after another until all columns are printed.
        class PageDef
        {
            public PageDef(Margins m, int count)
            {
                colstoprint = new List<object>(count);
                colwidths = new List<float>(count);
                colwidthsoverride = new List<float>(count);
                coltotalwidth = 0;
                margins = (Margins)m.Clone();
            }

            public IList colstoprint;
            public List<float> colwidths;
            public List<float> colwidthsoverride;
            public float coltotalwidth;
            public Margins margins;
        }
        IList<PageDef> pagesets;
        int currentpageset = 0;

        // class to hold settings for the PrintDialog presented to the user during
        // the print process
        public class PrintDialogSettingsClass
        {
            public bool AllowSelection = true;
            public bool AllowSomePages = true;
            public bool AllowCurrentPage = true;
            public bool AllowPrintToFile = false;
            public bool ShowHelp = true;
            public bool ShowNetwork = true;
            public bool UseEXDialog = true;
        }


        //---------------------------------------------------------------------
        // global variables
        //---------------------------------------------------------------------
        #region global variables

        // the data grid view we're printing
        DataGridView dgv = null;

        // print document
        PrintDocument printDoc = null;

        // print status items
        IList rowstoprint;
        IList colstoprint;          // divided into pagesets for printing
        int lastrowprinted = -1;
        int currentrow = -1;
        int fromPage = 0;
        int toPage = -1;
        const int maxPages = 2147483647;

        // page formatting options
        int pageHeight = 0;
        float staticheight = 0;
        float rowstartlocation = 0;
        int pageWidth = 0;
        int printWidth = 0;
        float rowheaderwidth = 0;
        int CurrentPage = 0;
        int totalpages;
        PrintRange printRange;

        // calculated values
        //private float headerHeight = 0;
        private float footerHeight = 0;
        private float pagenumberHeight = 0;
        private float colheaderheight = 0;
        private List<float> rowheights;
        private List<float> colwidths;
        private List<List<SizeF>> cellsizes;

        #endregion

        //---------------------------------------------------------------------
        // properties - settable by user
        //---------------------------------------------------------------------
        #region properties

        #region global properties

        /// <summary>
        /// OwnerDraw Event declaration. Callers can subscribe to this event to override the 
        /// cell drawing.
        /// </summary>
        public event CellOwnerDrawEventHandler OwnerDraw;

        /// <summary>
        /// provide an override for the print preview dialog "owner" field
        /// Note: Changed style for VS2005 compatibility
        /// </summary>
        //public Form Owner
        //{ get; set; }
        protected Form _Owner = null;
        public Form Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }

        /// <summary>
        /// provide an override for the print preview zoom setting
        /// Note: Changed style for VS2005 compatibility
        /// </summary>
        //public Double PrintPreviewZoom
        //{ get; set; }
        protected Double _PrintPreviewZoom = 1.0;
        public Double PrintPreviewZoom
        {
            get { return _PrintPreviewZoom; }
            set { _PrintPreviewZoom = value; }
        }


        /// <summary>
        /// expose printer settings to allow access to calling program
        /// </summary>
        public PrinterSettings PrintSettings
        {
            get { return printDoc.PrinterSettings; }
        }

        /// <summary>
        /// expose settings for the PrintDialog displayed to the user
        /// </summary>
        private PrintDialogSettingsClass printDialogSettings = new PrintDialogSettingsClass();
        public PrintDialogSettingsClass PrintDialogSettings
        {
            get { return printDialogSettings; }
        }

        /// <summary>
        /// Set Printer Name
        /// </summary>
        private String printerName;
        public String PrinterName
        {
            get { return printerName; }
            set { printerName = value; }
        }

        /// <summary>
        /// Allow access to the underlying print document
        /// </summary>
        public PrintDocument printDocument
        {
            get { return printDoc; }
            set { printDoc = value; }
        }

        /// <summary>
        /// Allow caller to set the upper-left corner icon used
        /// in the print preview dialog
        /// </summary>
        private Icon ppvIcon = null;
        public Icon PreviewDialogIcon
        {
            get { return ppvIcon; }
            set { ppvIcon = value; }
        }

        /// <summary>
        /// Allow caller to set print preview dialog
        /// </summary>
        private PrintPreviewDialog previewdialog = null;
        public PrintPreviewDialog PreviewDialog
        {
            get { return previewdialog; }
            set { previewdialog = value; }
        }

        /// <summary>
        /// Flag to control whether or not we print the Page Header
        /// </summary>
        private Boolean printHeader = true;
        public Boolean PrintHeader
        {
            get { return printHeader; }
            set { printHeader = value; }
        }

        /// <summary>
        /// Determine the height of the header
        /// </summary>
        private float HeaderHeight
        {
            get
            {
                float headerheight = 0;

                // add pageno height if we're printing it in the header and on a 
                // separate line
                if (pagenumberontop && !pagenumberonseparateline)
                    headerheight += pagenumberHeight;

                // Add in title and subtitle heights - this is sensitive to 
                // wether or not titles are printed on the current page
                headerheight += TitleHeight + SubTitleHeight;

                // Add in column header heights
                if ((bool)PrintColumnHeaders)
                {
                    headerheight += colheaderheight;
                }

                // return calculated height
                return headerheight;
            }
        }

        /// <summary>
        /// Flag to control whether or not we print the Page Footer
        /// </summary>
        private Boolean printFooter = true;
        public Boolean PrintFooter
        {
            get { return printFooter; }
            set { printFooter = value; }
        }

        /// <summary>
        /// Flag to control whether or not we print the Column Header line
        /// </summary>
        private Boolean? printColumnHeaders;
        public Boolean? PrintColumnHeaders
        {
            get { return printColumnHeaders; }
            set { printColumnHeaders = value; }
        }

        /// <summary>
        /// Flag to control whether or not we print the Column Header line
        /// Defaults to False to match previous functionality
        /// </summary>
        private Boolean? printRowHeaders = false;
        public Boolean? PrintRowHeaders
        {
            get { return printRowHeaders; }
            set { printRowHeaders = value; }
        }

        /// <summary>
        /// Flag to control whether rows are printed whole or if partial
        /// rows should be printed to fill the bottom of the page. Turn this
        /// "Off" (i.e. false) to print cells/rows deeper than one page
        /// </summary>
        private Boolean keepRowsTogether = true;
        public Boolean KeepRowsTogether
        {
            get { return keepRowsTogether; }
            set { keepRowsTogether = value; }
        }

        #endregion

        // Title
        #region title properties

        // override flag
        bool overridetitleformat = false;

        // formatted height of title
        float titleheight = 0;

        /// <summary>
        /// Title for this report. Default is empty.
        /// </summary>
        private String title;
        public String Title
        {
            get { return title; }
            set
            {
                title = value;
                if (docName == null)
                {
                    printDoc.DocumentName = value;
                }
            }
        }

        /// <summary>
        /// Name of the document. Default is report title (can be empty)
        /// </summary>
        private String docName;
        public String DocName
        {
            get { return docName; }
            set { printDoc.DocumentName = value; docName = value; }
        }

        /// <summary>
        /// Font for the title. Default is Tahoma, 18pt.
        /// </summary>
        private Font titlefont;
        public Font TitleFont
        {
            get { return titlefont; }
            set { titlefont = value; }
        }

        /// <summary>
        /// Foreground color for the title. Default is Black
        /// </summary>
        private Color titlecolor;
        public Color TitleColor
        {
            get { return titlecolor; }
            set { titlecolor = value; }
        }

        /// <summary>
        /// Allow override of the header cell format object
        /// </summary>
        private StringFormat titleformat;
        public StringFormat TitleFormat
        {
            get { return titleformat; }
            set { titleformat = value; overridetitleformat = true; }
        }

        /// <summary>
        /// Allow the user to override the title string alignment. Default value is 
        /// Alignment - Near; 
        /// </summary>
        public StringAlignment TitleAlignment
        {
            get { return titleformat.Alignment; }
            set
            {
                titleformat.Alignment = value;
                overridetitleformat = true;
            }
        }

        /// <summary>
        /// Allow the user to override the title string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        public StringFormatFlags TitleFormatFlags
        {
            get { return titleformat.FormatFlags; }
            set
            {
                titleformat.FormatFlags = value;
                overridetitleformat = true;
            }
        }

        /// <summary>
        /// Control where in the document the title prints
        /// </summary>
        private PrintLocation titleprint = PrintLocation.All;
        public PrintLocation TitlePrint
        {
            get { return titleprint; }
            set { titleprint = value; }
        }

        /// <summary>
        /// Return the title height based whether to print it or not
        /// </summary>
        private float TitleHeight
        {
            get
            {
                if (PrintLocation.All == TitlePrint)
                    return titleheight + titlespacing;

                if ((PrintLocation.FirstOnly == TitlePrint) && (1 == CurrentPage))
                    return titleheight + titlespacing;

                return 0;
            }
        }

        /// <summary>
        /// Mandatory spacing between the grid and the footer
        /// </summary>
        private float titlespacing;
        public float TitleSpacing
        {
            get { return titlespacing; }
            set { titlespacing = value; }
        }

        #endregion

        // SubTitle
        #region subtitle properties

        // override flat
        bool overridesubtitleformat = false;

        // formatted height of subtitle
        float subtitleheight = 0;

        /// <summary>
        /// SubTitle for this report. Default is empty.
        /// </summary>
        private String subtitle;
        public String SubTitle
        {
            get { return subtitle; }
            set { subtitle = value; }
        }

        /// <summary>
        /// Font for the subtitle. Default is Tahoma, 12pt.
        /// </summary>
        private Font subtitlefont;
        public Font SubTitleFont
        {
            get { return subtitlefont; }
            set { subtitlefont = value; }
        }

        /// <summary>
        /// Foreground color for the subtitle. Default is Black
        /// </summary>
        private Color subtitlecolor;
        public Color SubTitleColor
        {
            get { return subtitlecolor; }
            set { subtitlecolor = value; }
        }

        /// <summary>
        /// Allow override of the header cell format object
        /// </summary>
        private StringFormat subtitleformat;
        public StringFormat SubTitleFormat
        {
            get { return subtitleformat; }
            set { subtitleformat = value; overridesubtitleformat = true; }
        }

        /// <summary>
        /// Allow the user to override the subtitle string alignment. Default value is 
        /// Alignment - Near; 
        /// </summary>
        public StringAlignment SubTitleAlignment
        {
            get { return subtitleformat.Alignment; }
            set
            {
                subtitleformat.Alignment = value;
                overridesubtitleformat = true;
            }
        }

        /// <summary>
        /// Allow the user to override the subtitle string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        public StringFormatFlags SubTitleFormatFlags
        {
            get { return subtitleformat.FormatFlags; }
            set
            {
                subtitleformat.FormatFlags = value;
                overridesubtitleformat = true;
            }
        }

        /// <summary>
        /// Control where in the document the title prints
        /// </summary>
        private PrintLocation subtitleprint = PrintLocation.All;
        public PrintLocation SubTitlePrint
        {
            get { return subtitleprint; }
            set { subtitleprint = value; }
        }

        /// <summary>
        /// Return the title height based whether to print it or not
        /// </summary>
        private float SubTitleHeight
        {
            get
            {
                if (PrintLocation.All == SubTitlePrint)
                    return subtitleheight + subtitlespacing;

                if ((PrintLocation.FirstOnly == SubTitlePrint) && (1 == CurrentPage))
                    return subtitleheight + subtitlespacing;

                return 0;
            }
        }

        /// <summary>
        /// Mandatory spacing between the grid and the footer
        /// </summary>
        private float subtitlespacing;
        public float SubTitleSpacing
        {
            get { return subtitlespacing; }
            set { subtitlespacing = value; }
        }
        #endregion

        // Footer
        #region footer properties

        // override flag
        bool overridefooterformat = false;

        /// <summary>
        /// footer for this report. Default is empty.
        /// </summary>
        private String footer;
        public String Footer
        {
            get { return footer; }
            set { footer = value; }
        }

        /// <summary>
        /// Font for the footer. Default is Tahoma, 10pt.
        /// </summary>
        private Font footerfont;
        public Font FooterFont
        {
            get { return footerfont; }
            set { footerfont = value; }
        }

        /// <summary>
        /// Foreground color for the footer. Default is Black
        /// </summary>
        private Color footercolor;
        public Color FooterColor
        {
            get { return footercolor; }
            set { footercolor = value; }
        }

        /// <summary>
        /// Allow override of the header cell format object
        /// </summary>
        private StringFormat footerformat;
        public StringFormat FooterFormat
        {
            get { return footerformat; }
            set { footerformat = value; overridefooterformat = true; }
        }

        /// <summary>
        /// Allow the user to override the footer string alignment. Default value is 
        /// Alignment - Center; 
        /// </summary>
        public StringAlignment FooterAlignment
        {
            get { return footerformat.Alignment; }
            set
            {
                footerformat.Alignment = value;
                overridefooterformat = true;
            }
        }

        /// <summary>
        /// Allow the user to override the footer string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        public StringFormatFlags FooterFormatFlags
        {
            get { return footerformat.FormatFlags; }
            set
            {
                footerformat.FormatFlags = value;
                overridefooterformat = true;
            }
        }

        /// <summary>
        /// Mandatory spacing between the grid and the footer
        /// </summary>
        private float footerspacing;
        public float FooterSpacing
        {
            get { return footerspacing; }
            set { footerspacing = value; }
        }

        #endregion

        // Page Numbering
        #region page number properties

        // override flag
        bool overridepagenumberformat = false;

        /// <summary>
        /// Include page number in the printout. Default is true.
        /// </summary>
        private bool pageno = true;
        public bool PageNumbers
        {
            get { return pageno; }
            set { pageno = value; }
        }

        /// <summary>
        /// Font for the page number, Default is Tahoma, 8pt.
        /// </summary>
        private Font pagenofont;
        public Font PageNumberFont
        {
            get { return pagenofont; }
            set { pagenofont = value; }
        }

        /// <summary>
        /// Text color (foreground) for the page number. Default is Black
        /// </summary>
        private Color pagenocolor;
        public Color PageNumberColor
        {
            get { return pagenocolor; }
            set { pagenocolor = value; }
        }

        /// <summary>
        /// Allow override of the header cell format object
        /// </summary>
        private StringFormat pagenumberformat;
        public StringFormat PageNumberFormat
        {
            get { return pagenumberformat; }
            set { pagenumberformat = value; overridepagenumberformat = true; }
        }

        /// <summary>
        /// Allow the user to override the page number string alignment. Default value is 
        /// Alignment - Near; 
        /// </summary>
        public StringAlignment PageNumberAlignment
        {
            get { return pagenumberformat.Alignment; }
            set
            {
                pagenumberformat.Alignment = value;
                overridepagenumberformat = true;
            }
        }

        /// <summary>
        /// Allow the user to override the pagenumber string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        public StringFormatFlags PageNumberFormatFlags
        {
            get { return pagenumberformat.FormatFlags; }
            set
            {
                pagenumberformat.FormatFlags = value;
                overridepagenumberformat = true;
            }
        }

        /// <summary>
        /// Allow the user to select whether to have the page number at the top or bottom
        /// of the page. Default is false: page numbers on the bottom of the page
        /// </summary>
        private bool pagenumberontop = false;
        public bool PageNumberInHeader
        {
            get { return pagenumberontop; }
            set { pagenumberontop = value; }
        }

        /// <summary>
        /// Should the page number be printed on a separate line, or printed on the
        /// same line as the header / footer? Default is false;
        /// </summary>
        private bool pagenumberonseparateline = false;
        public bool PageNumberOnSeparateLine
        {
            get { return pagenumberonseparateline; }
            set { pagenumberonseparateline = value; }
        }

        /// <summary>
        /// Show the total page number as n of total 
        /// </summary>
        private bool showtotalpagenumber = false;
        public bool ShowTotalPageNumber
        {
            get { return showtotalpagenumber; }
            set { showtotalpagenumber = value; }
        }

        /// <summary>
        /// Text separating page number and total page number. Default is ' of '.
        /// </summary>
        private String pageseparator = " of ";
        public String PageSeparator
        {
            get { return pageseparator; }
            set { pageseparator = value; }
        }

        private String pagetext = "Page ";
        public String PageText
        {
            get { return pagetext; }
            set { pagetext = value; }
        }

        private String parttext = " - Part ";
        public String PartText
        {
            get { return parttext; }
            set { parttext = value; }
        }

        #endregion

        // Header Cell Printing 
        #region header cell properties

        private DataGridViewCellStyle rowheaderstyle;
        public DataGridViewCellStyle RowHeaderCellStyle
        {
            get { return rowheaderstyle; }
            set { rowheaderstyle = value; }
        }

        /// <summary>
        /// Allow override of the row header cell format object
        /// </summary>
        private StringFormat rowheadercellformat = null;
        public StringFormat GetRowHeaderCellFormat(DataGridView grid)
        {
            // get default values from provided data grid view, but only
            // if we don't already have a header cell format
            if ((null != grid) && (null == rowheadercellformat))
            {
                buildstringformat(ref rowheadercellformat, grid.Rows[0].HeaderCell.InheritedStyle,
                    headercellalignment, StringAlignment.Near, headercellformatflags,
                    StringTrimming.Word);
            }

            // if we still don't have a header cell format, create an empty
            if (null == rowheadercellformat)
                rowheadercellformat = new StringFormat(headercellformatflags);

            return rowheadercellformat;
        }

        /// <summary>
        /// Default value to show in the row header cell if no value is provided in the DataGridView.
        /// Defaults to one tab space
        /// </summary>
        private String rowheadercelldefaulttext = "\t";
        public String RowHeaderCellDefaultText
        {
            get { return rowheadercelldefaulttext; }
            set { rowheadercelldefaulttext = value; }
        }

        /// <summary>
        /// Allow override of the header cell format object
        /// </summary>
        private Dictionary<string, DataGridViewCellStyle> columnheaderstyles =
            new Dictionary<string, DataGridViewCellStyle>();
        public Dictionary<string, DataGridViewCellStyle> ColumnHeaderStyles
        {
            get { return columnheaderstyles; }
        }

        /// <summary>
        /// Allow override of the header cell format object
        /// </summary>
        private StringFormat columnheadercellformat = null;
        public StringFormat GetColumnHeaderCellFormat(DataGridView grid)
        {
            // get default values from provided data grid view, but only
            // if we don't already have a header cell format
            if ((null != grid) && (null == columnheadercellformat))
            {
                buildstringformat(ref columnheadercellformat, grid.Columns[0].HeaderCell.InheritedStyle,
                    headercellalignment, StringAlignment.Near, headercellformatflags,
                    StringTrimming.Word);
            }

            // if we still don't have a header cell format, create an empty
            if (null == columnheadercellformat)
                columnheadercellformat = new StringFormat(headercellformatflags);

            return columnheadercellformat;
        }

        /// <summary>
        /// Deprecated - use HeaderCellFormat
        /// Allow the user to override the header cell string alignment. Default value is 
        /// Alignment - Near; 
        /// </summary>
        private StringAlignment headercellalignment;
        public StringAlignment HeaderCellAlignment
        {
            get { return headercellalignment; }
            set { headercellalignment = value; }
        }

        /// <summary>
        /// Deprecated - use HeaderCellFormat
        /// Allow the user to override the header cell string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        private StringFormatFlags headercellformatflags;
        public StringFormatFlags HeaderCellFormatFlags
        {
            get { return headercellformatflags; }
            set { headercellformatflags = value; }
        }
        #endregion

        // Individual Cell Printing
        #region cell properties

        /// <summary>
        /// Allow override of the cell printing format
        /// </summary>
        private StringFormat cellformat = null;
        public StringFormat GetCellFormat(DataGridView grid)
        {
            // get default values from provided data grid view, but only
            // if we don't already have a cell format
            if ((null != grid) && (null == cellformat))
            {
                buildstringformat(ref cellformat, grid.Rows[0].Cells[0].InheritedStyle,
                    cellalignment, StringAlignment.Near, cellformatflags,
                    StringTrimming.Word);
            }

            // if we still don't have a cell format, create an empty
            if (null == cellformat)
                cellformat = new StringFormat(cellformatflags);

            return cellformat;
        }

        /// <summary>
        /// Deprecated - use GetCellFormat
        /// Allow the user to override the cell string alignment. Default value is 
        /// Alignment - Near; 
        /// </summary>
        private StringAlignment cellalignment;
        public StringAlignment CellAlignment
        {
            get { return cellalignment; }
            set { cellalignment = value; }
        }

        /// <summary>
        /// Deprecated - use GetCellFormat
        /// Allow the user to override the cell string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        private StringFormatFlags cellformatflags;
        public StringFormatFlags CellFormatFlags
        {
            get { return cellformatflags; }
            set { cellformatflags = value; }
        }

        /// <summary>
        /// allow the user to override the column width calcs with their own defaults
        /// </summary>
        private List<float> colwidthsoverride = new List<float>();
        private Dictionary<string, float> publicwidthoverrides = new Dictionary<string, float>();
        public Dictionary<string, float> ColumnWidths
        {
            get { return publicwidthoverrides; }
        }

        /// <summary>
        /// Allow per column style overrides
        /// </summary>
        private Dictionary<string, DataGridViewCellStyle> colstyles =
            new Dictionary<string, DataGridViewCellStyle>();
        public Dictionary<string, DataGridViewCellStyle> ColumnStyles
        {
            get { return colstyles; }
        }

        /// <summary>
        /// Allow per column style overrides
        /// </summary>
        private Dictionary<string, DataGridViewCellStyle> altrowcolstyles =
            new Dictionary<string, DataGridViewCellStyle>();
        public Dictionary<string, DataGridViewCellStyle> AlternatingRowColumnStyles
        {
            get { return altrowcolstyles; }
        }

        #endregion

        // Page Level Properties
        #region page level properties

        /// <summary>
        /// Page margins override. Default is (60, 60, 40, 40)
        /// </summary>
        public Margins PrintMargins
        {
            get { return PageSettings.Margins; }
            set { PageSettings.Margins = value; }
        }

        /// <summary>
        /// Expose the printdocument default page settings to the caller
        /// </summary>
        public PageSettings PageSettings
        {
            get { return printDoc.DefaultPageSettings; }
        }

        /// <summary>
        /// Spread the columns porportionally accross the page. Default is false.
        /// Deprecated. Please use the ColumnWidth property
        /// </summary>
        private bool porportionalcolumns = false;
        public bool PorportionalColumns
        {
            get { return porportionalcolumns; }
            set
            {
                porportionalcolumns = value;
                if (porportionalcolumns)
                    ColumnWidth = ColumnWidthSetting.Porportional;
                else
                    ColumnWidth = ColumnWidthSetting.CellWidth;
            }
        }

        /// <summary>
        /// Center the table on the page. 
        /// </summary>
        private Alignment tablealignment = Alignment.NotSet;
        public Alignment TableAlignment
        {
            get { return tablealignment; }
            set { tablealignment = value; }
        }

        /// <summary>
        /// Change the default row height to either the height of the string or the size of 
        /// the cell. Added for image cell handling; set to CellHeight for image cells
        /// </summary>
        public enum RowHeightSetting { DataHeight, CellHeight }
        private RowHeightSetting _rowheight = RowHeightSetting.DataHeight;
        public RowHeightSetting RowHeight
        {
            get { return _rowheight; }
            set { _rowheight = value; }
        }

        /// <summary>
        /// Change the default column width to be spread porportionally accross the page,
        /// to the size of the grid cell or the size of the formatted data string.
        /// Set to CellWidth for image cells.
        /// </summary>
        public enum ColumnWidthSetting { DataWidth, CellWidth, Porportional }
        private ColumnWidthSetting _rowwidth = ColumnWidthSetting.CellWidth;
        public ColumnWidthSetting ColumnWidth
        {
            get { return _rowwidth; }
            set
            {
                _rowwidth = value;
                if (value == ColumnWidthSetting.Porportional)
                    porportionalcolumns = true;
                else
                    porportionalcolumns = false;
            }
        }


        #endregion

        // Utility Functions
        #region
        /// <summary>
        /// calculate the print preview window width to show the entire page
        /// </summary>
        /// <returns></returns>
        private int PreviewDisplayWidth()
        {
            double displayWidth = printDoc.DefaultPageSettings.Bounds.Width
                + 3 * printDoc.DefaultPageSettings.HardMarginY;
            return (int)(displayWidth * PrintPreviewZoom);
        }

        /// <summary>
        /// calculate the print preview window height to show the entire page
        /// </summary>
        /// <returns></returns>
        private int PreviewDisplayHeight()
        {
            double displayHeight = printDoc.DefaultPageSettings.Bounds.Height
                + 3 * printDoc.DefaultPageSettings.HardMarginX;

            return (int)(displayHeight * PrintPreviewZoom);
        }

        /// <summary>
        /// Invoke any provided cell owner draw routines
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCellOwnerDraw(DGVCellDrawingEventArgs e)
        {
            if (null != OwnerDraw)
                OwnerDraw(this, e);
        }

        /// <summary>
        /// Given a row and column, get the current grid cell style, including our local 
        /// overrides
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        protected DataGridViewCellStyle GetStyle(DataGridViewRow row, DataGridViewColumn col)
        {
            // set initial default
            DataGridViewCellStyle colstyle = row.Cells[col.Index].InheritedStyle.Clone();

            // check odd or even rows for our override
            if (0 == (row.Index & 1))
            {
                if (ColumnStyles.ContainsKey(col.Name))
                {
                    colstyle = ColumnStyles[col.Name];
                }
            }
            else
            {
                if (AlternatingRowColumnStyles.ContainsKey(col.Name))
                {
                    colstyle = AlternatingRowColumnStyles[col.Name];
                }
            }

            return colstyle;
        }

        #endregion

        #endregion

        //---------------------------------------------------------------------
        // Constructor
        //---------------------------------------------------------------------
        /// <summary>
        /// Constructor for DGVPrinter
        /// </summary>
        public DGVPrinter()
        {
            // create print document
            printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            printDoc.BeginPrint += new PrintEventHandler(printDoc_BeginPrint);
            PrintMargins = new Margins(60, 60, 40, 40);

            // set default fonts
            pagenofont = new Font("Tahoma", 8, FontStyle.Regular, GraphicsUnit.Point);
            pagenocolor = Color.Black;
            titlefont = new Font("Tahoma", 18, FontStyle.Bold, GraphicsUnit.Point);
            titlecolor = Color.Black;
            subtitlefont = new Font("Tahoma", 12, FontStyle.Bold, GraphicsUnit.Point);
            subtitlecolor = Color.Black;
            footerfont = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Point);
            footercolor = Color.Black;

            // default spacing
            titlespacing = 0;
            subtitlespacing = 0;
            footerspacing = 0;

            // Create string formatting objects
            buildstringformat(ref titleformat, null, StringAlignment.Center, StringAlignment.Center,
                StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
            buildstringformat(ref subtitleformat, null, StringAlignment.Center, StringAlignment.Center,
                StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
            buildstringformat(ref footerformat, null, StringAlignment.Center, StringAlignment.Center,
                StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);
            buildstringformat(ref pagenumberformat, null, StringAlignment.Far, StringAlignment.Center,
                StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip, StringTrimming.Word);

            // Set these formatting objects to null to flag whether or not they were set by the caller
            columnheadercellformat = null;
            rowheadercellformat = null;
            cellformat = null;

            // Print Preview properties
            Owner = null;
            PrintPreviewZoom = 1.0;

            // Deprecated properties - retain for backwards compatibility
            headercellalignment = StringAlignment.Near;
            headercellformatflags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            cellalignment = StringAlignment.Near;
            cellformatflags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
        }


        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        // Primary Interface - Presents a dialog and then prints or previews the 
        // indicated data grid view
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------

        /// <summary>
        /// Start the printing process, print to a printer.
        /// </summary>
        /// <param name="dgv">The DataGridView to print</param>
        /// NOTE: Any changes to this method also need to be done in PrintPreviewDataGridView
        public void PrintDataGridView(DataGridView dgv)
        {
            if (null == dgv) throw new Exception("Null Parameter passed to DGVPrinter.");
            if (!(dgv is DataGridView))
                throw new Exception("Invalid Parameter passed to DGVPrinter.");

            // save the datagridview we're printing
            this.dgv = dgv;

            // display dialog and print
            if (DialogResult.OK == DisplayPrintDialog())
            {
                SetupPrint();
                printDoc.Print();
            }
        }

        /// <summary>
        /// Start the printing process, print to a print preview dialog
        /// </summary>
        /// <param name="dgv">The DataGridView to print</param>
        /// NOTE: Any changes to this method also need to be done in PrintDataGridView
        public void PrintPreviewDataGridView(DataGridView dgv)
        {
            if (null == dgv) throw new Exception("Null Parameter passed to DGVPrinter.");
            if (!(dgv is DataGridView))
                throw new Exception("Invalid Parameter passed to DGVPrinter.");

            // save the datagridview we're printing
            this.dgv = dgv;

            // display dialog and print
            if (DialogResult.OK == DisplayPrintDialog())
            {
                PrintPreviewNoDisplay(dgv);
            }
        }

        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        // Alternative Interface. In order to set the print information correctly
        // either the DisplayPrintDialog() routine must be called, OR the 
        // PrintDocument (and PrinterSettings) must be Handled through calling
        // PrintDialog separately.
        //
        // Once the PrintDocument has been setup, the PrintNoDisplay() and/or
        // PrintPreviewNoDisplay() routines can be called to print multiple
        // DataGridViews using the same print setup.
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------

        /// <summary>
        /// Display a printdialog and return the result. Either this method or 
        /// the equivalent must be done prior to calling either of the PrintNoDisplay
        /// or PrintPreviewNoDisplay methods.
        /// </summary>
        /// <returns></returns>
        public DialogResult DisplayPrintDialog()
        {
            // create new print dialog and set options
            PrintDialog pd = new PrintDialog();
            pd.UseEXDialog = printDialogSettings.UseEXDialog;
            pd.AllowSelection = printDialogSettings.AllowSelection;
            pd.AllowSomePages = printDialogSettings.AllowSomePages;
            pd.AllowCurrentPage = printDialogSettings.AllowCurrentPage;
            pd.AllowPrintToFile = printDialogSettings.AllowPrintToFile;
            pd.ShowHelp = printDialogSettings.ShowHelp;
            pd.ShowNetwork = printDialogSettings.ShowNetwork;

            // setup print dialog with internal setttings
            pd.Document = printDoc;
            if (!String.IsNullOrEmpty(printerName))
                printDoc.PrinterSettings.PrinterName = printerName;

            // Ensure default landscape setting and papersize setting match print dialog's
            printDoc.DefaultPageSettings.Landscape = pd.PrinterSettings.DefaultPageSettings.Landscape;
            printDoc.DefaultPageSettings.PaperSize =
                new PaperSize(pd.PrinterSettings.DefaultPageSettings.PaperSize.PaperName,
                    pd.PrinterSettings.DefaultPageSettings.PaperSize.Width,
                    pd.PrinterSettings.DefaultPageSettings.PaperSize.Height);

            // show the dialog and display the result
            return pd.ShowDialog();
        }

        /// <summary>
        /// Print the provided grid view. Either DisplayPrintDialog() or it's equivalent
        /// setup must be completed prior to calling this routine
        /// </summary>
        /// <param name="dgv"></param>
        public void PrintNoDisplay(DataGridView dgv)
        {
            if (null == dgv) throw new Exception("Null Parameter passed to DGVPrinter.");
            if (!(dgv is DataGridView))
                throw new Exception("Invalid Parameter passed to DGVPrinter.");

            // save the grid we're printing
            this.dgv = dgv;

            // setup and do printing
            SetupPrint();
            printDoc.Print();
        }

        /// <summary>
        /// Preview the provided grid view. Either DisplayPrintDialog() or it's equivalent
        /// setup must be completed prior to calling this routine
        /// </summary>
        /// <param name="dgv"></param>
        public void PrintPreviewNoDisplay(DataGridView dgv)
        {
            if (null == dgv) throw new Exception("Null Parameter passed to DGVPrinter.");
            if (!(dgv is DataGridView))
                throw new Exception("Invalid Parameter passed to DGVPrinter.");

            // save the grid we're printing
            this.dgv = dgv;

            // display the preview dialog
            SetupPrint();

            // if the caller hasn't provided a print preview dialog, then create one
            if (null == PreviewDialog)
                PreviewDialog = new PrintPreviewDialog();

            // set up dialog for preview
            PreviewDialog.Document = printDoc;
            PreviewDialog.UseAntiAlias = true;
            PreviewDialog.Owner = Owner;
            PreviewDialog.PrintPreviewControl.Zoom = PrintPreviewZoom;
            PreviewDialog.Width = PreviewDisplayWidth();
            PreviewDialog.Height = PreviewDisplayHeight();

            if (null != ppvIcon)
                PreviewDialog.Icon = ppvIcon;

            // show the dialog
            PreviewDialog.ShowDialog();
        }


        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        // Print Process Interface Methods
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------

        public bool EmbeddedPrint(DataGridView dgv, Graphics g, Rectangle area)
        {
            // verify we've been set up properly
            if ((null == dgv) || (null == g))
                throw new Exception("Null Parameter passed to DGVPrinter.");

            // save the grid we're printing
            this.dgv = dgv;

            //-----------------------------------------------------------------
            // do a mini version of SetupPrint for printing into an already 
            // provided graphics context rather than owning the entire print process
            //-----------------------------------------------------------------
            if (null == PrintColumnHeaders)
                PrintColumnHeaders = dgv.Columns[0].HeaderCell.Visible;

            if (null == PrintRowHeaders)
                PrintRowHeaders = dgv.RowHeadersVisible;

            // Set the default row header style where we don't have an override
            if (null == RowHeaderCellStyle)
                RowHeaderCellStyle = dgv.Rows[0].HeaderCell.InheritedStyle;

            // set margins so we print within the provided area
            Margins saveMargins = PrintMargins;
            PrintMargins.Top = area.Top;
            PrintMargins.Bottom = 0;
            PrintMargins.Left = area.Left;
            PrintMargins.Right = 0;

            // set "page" height and width to our destination area
            pageHeight = area.Height + area.Top;
            printWidth = area.Width;
            pageWidth = area.Width + area.Left;

            // initially assume we'll eventually print all records
            fromPage = 0;
            toPage = maxPages;

            // force 'off' header and footer
            PrintHeader = false;
            PrintFooter = false;

            // set default cell formats
            if (null == columnheadercellformat)
                buildstringformat(ref columnheadercellformat, dgv.Columns[0].HeaderCell.InheritedStyle,
                    headercellalignment, StringAlignment.Near, headercellformatflags,
                    StringTrimming.Word);
            if (null == rowheadercellformat)
                buildstringformat(ref rowheadercellformat, RowHeaderCellStyle,
                    headercellalignment, StringAlignment.Near, headercellformatflags,
                    StringTrimming.Word);
            if (null == cellformat)
                buildstringformat(ref cellformat, dgv.DefaultCellStyle,
                    cellalignment, StringAlignment.Near, cellformatflags,
                    StringTrimming.Word);

            //-----------------------------------------------------------------
            // Determine what's going to be printed and set the columns to print
            //-----------------------------------------------------------------
            SetupPrintRange();
            SetupColumnOrder();

            //-----------------------------------------------------------------
            // Now that we know what we're printing, measure the print area and
            // count the pages.
            //-----------------------------------------------------------------

            // Measure the print area
            measureprintarea(g);

            // Count the pages
            totalpages = TotalPages();

            // set counter values
            currentpageset = 0;
            lastrowprinted = -1;
            CurrentPage = 0;

            // call the print engine
            return PrintPage(g);

            // reset printer margins
            // PrintMargins = saveMargins;
        }

        /// <summary>
        /// BeginPrint Event Handler
        /// Set values at start of print run
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printDoc_BeginPrint(object sender, PrintEventArgs e)
        {
            // reset counters since we'll go through this twice if we print from preview
            currentpageset = 0;
            lastrowprinted = -1;
            CurrentPage = 0;
        }

        /// <summary>
        /// PrintPage event handler. This routine prints one page. It will
        /// skip non-printable pages if the user selected the "some pages" option
        /// on the print dialog.
        /// </summary>
        /// <param name="sender">default object from windows</param>
        /// <param name="e">Event info from Windows about the printing</param>
        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.HasMorePages = PrintPage(e.Graphics);
        }


        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        // Internal Methods
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------

        /// <summary>
        /// Set up the print job. Save information from print dialog
        /// and print document for easy access. Also sets up the rows
        /// and columns that will be printed. At this point, we're 
        /// collecting all columns in colstoprint. This will be broken
        /// up into pagesets later on 
        /// </summary>
        void SetupPrint()
        {
            if (null == PrintColumnHeaders)
                PrintColumnHeaders = dgv.Columns[0].HeaderCell.Visible;

            if (null == PrintRowHeaders)
                PrintRowHeaders = dgv.RowHeadersVisible;

            // Set the default row header style where we don't have an override
            if (null == RowHeaderCellStyle)
                RowHeaderCellStyle = dgv.Rows[0].HeaderCell.InheritedStyle;

            /* Functionality to come - redo of styling
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                // Set the default column styles where we've not been given an override
                if (!ColumnStyles.ContainsKey(col.Name))
                    ColumnStyles[col.Name] = dgv.Columns[col.Name].InheritedStyle;

                // Set the default column header styles where we don't have an override
                if (!ColumnHeaderStyles.ContainsKey(col.Name))
                    ColumnHeaderStyles[col.Name] = dgv.Columns[col.Name].HeaderCell.InheritedStyle;
            }
            */

            //-----------------------------------------------------------------
            // Set row and column headercell and normal cell print formats if they were not
            // explicitly set by the caller
            //-----------------------------------------------------------------
            if (null == columnheadercellformat)
                buildstringformat(ref columnheadercellformat, dgv.Columns[0].HeaderCell.InheritedStyle,
                    headercellalignment, StringAlignment.Near, headercellformatflags,
                    StringTrimming.Word);
            if (null == rowheadercellformat)
                buildstringformat(ref rowheadercellformat, RowHeaderCellStyle,
                    headercellalignment, StringAlignment.Near, headercellformatflags,
                    StringTrimming.Word);
            if (null == cellformat)
                buildstringformat(ref cellformat, dgv.DefaultCellStyle,
                    cellalignment, StringAlignment.Near, cellformatflags,
                    StringTrimming.Word);

            //-----------------------------------------------------------------
            // get info on the limits of the printer's actual print area available. Convert
            // to int's to work with margins.
            //-----------------------------------------------------------------
            int hardx = (int)Math.Round(printDoc.DefaultPageSettings.HardMarginX);
            int hardy = (int)Math.Round(printDoc.DefaultPageSettings.HardMarginY);
            int printareawidth;
            if (printDoc.DefaultPageSettings.Landscape)
                printareawidth = (int)Math.Round(printDoc.DefaultPageSettings.PrintableArea.Height);
            else
                printareawidth = (int)Math.Round(printDoc.DefaultPageSettings.PrintableArea.Width);

            //-----------------------------------------------------------------
            // set the print area we're working within
            //-----------------------------------------------------------------

            pageHeight = printDoc.DefaultPageSettings.Bounds.Height;
            pageWidth = printDoc.DefaultPageSettings.Bounds.Width;

            //-----------------------------------------------------------------
            // Set the printable area: margins and pagewidth
            //-----------------------------------------------------------------

            // Set initial printer margins 
            PrintMargins = printDoc.DefaultPageSettings.Margins;

            // adjust for when the margins are less than the printer's hard x/y limits
            PrintMargins.Right = (hardx > PrintMargins.Right) ? hardx : PrintMargins.Right;
            PrintMargins.Left = (hardx > PrintMargins.Left) ? hardx : PrintMargins.Left;
            PrintMargins.Top = (hardy > PrintMargins.Top) ? hardy : PrintMargins.Top;
            PrintMargins.Bottom = (hardy > PrintMargins.Bottom) ? hardy : PrintMargins.Bottom;

            // Now, we can calc default print width, again, respecting the printer's limitations
            printWidth = pageWidth - PrintMargins.Left - PrintMargins.Right;
            printWidth = (printWidth > printareawidth) ? printareawidth : printWidth;

            //-----------------------------------------------------------------
            // Figure out which pages / rows to print
            //-----------------------------------------------------------------

            // save print range 
            printRange = printDoc.PrinterSettings.PrintRange;

            // pages to print handles "some pages" option
            if (PrintRange.SomePages == printRange)
            {
                // set limits to only print some pages
                fromPage = printDoc.PrinterSettings.FromPage;
                toPage = printDoc.PrinterSettings.ToPage;
            }
            else
            {
                // set extremes so that we'll print all pages
                fromPage = 0;
                toPage = maxPages;
            }

            //-----------------------------------------------------------------
            // Determine what's going to be printed
            //-----------------------------------------------------------------
            SetupPrintRange();

            //-----------------------------------------------------------------
            // Determine the order of columns to be printed
            //-----------------------------------------------------------------
            SetupColumnOrder();

            //-----------------------------------------------------------------
            // Now that we know what we're printing, measure the print area and
            // count the pages.
            //-----------------------------------------------------------------

            // Measure the print area
            measureprintarea(printDoc.PrinterSettings.CreateMeasurementGraphics());

            // Count the pages
            totalpages = TotalPages();

        }

        /// <summary>
        /// Reorder columns based on Display Index (if the programmer or user has
        /// changed the column display order we want to respect it in the printout)
        /// If the right-to-left property is set, then reverse the column order
        /// </summary>
        private void SetupColumnOrder()
        {
            int rev = 1;
            if (RightToLeft.Yes == dgv.RightToLeft) rev = -1;
            SortedList displayorderlist = new SortedList(colstoprint.Count);
            foreach (DataGridViewColumn col in colstoprint) displayorderlist.Add(rev * col.DisplayIndex, col);
            colstoprint.Clear();
            foreach (object item in displayorderlist.Values) colstoprint.Add(item);

            // Adjust override list to have the same number of entries as colstoprint
            foreach (DataGridViewColumn col in colstoprint)
                if (publicwidthoverrides.ContainsKey(col.Name))
                    colwidthsoverride.Add(publicwidthoverrides[col.Name]);
                else
                    colwidthsoverride.Add(-1);
        }

        /// <summary>
        /// Determine the print range based on dialog selections and user input
        /// </summary>
        private void SetupPrintRange()
        {
            //-----------------------------------------------------------------
            // set up the rows and columns to print
            //
            // Note: The "Selectedxxxx" lists in the datagridview are 'stacks' that
            //  have the selected items pushed in the *in the order they were selected*
            //  i.e. not the order you want to print them in!
            //-----------------------------------------------------------------

            // rows to print (handles "selection" and "current page" options
            if (PrintRange.Selection == printRange)
            {
                SortedList temprowstoprint;
                SortedList tempcolstoprint;

                //if DGV has rows selected, it's easy, selected rows and all visible columns
                if (0 != dgv.SelectedRows.Count)
                {
                    // sort the rows into index order
                    temprowstoprint = new SortedList(dgv.SelectedRows.Count);
                    foreach (DataGridViewRow row in dgv.SelectedRows)
                        if (row.Visible && !row.IsNewRow)
                            temprowstoprint.Add(row.Index, row);

                    IEnumerator ie = temprowstoprint.Values.GetEnumerator();

                    rowstoprint = new List<object>(temprowstoprint.Count);
                    foreach (object item in temprowstoprint.Values) rowstoprint.Add(item);

                    colstoprint = new List<object>(dgv.Columns.Count);
                    foreach (DataGridViewColumn col in dgv.Columns) if (col.Visible) colstoprint.Add(col);
                }
                // if selected columns, then all rows, and selected columns
                else if (0 != dgv.SelectedColumns.Count)
                {
                    rowstoprint = dgv.Rows;

                    tempcolstoprint = new SortedList(dgv.SelectedColumns.Count);
                    foreach (DataGridViewRow row in dgv.SelectedColumns)
                        if (row.Visible && !row.IsNewRow)
                            tempcolstoprint.Add(row.Index, row);

                    colstoprint = new List<object>(tempcolstoprint.Count);
                    foreach (object item in tempcolstoprint.Values) colstoprint.Add(item);
                }
                // we just have a bunch of selected cells so we have to do some work
                else
                {
                    // set up sorted lists. the selectedcells method does not guarantee
                    // that the cells will always be in left-right top-bottom order. 
                    temprowstoprint = new SortedList(dgv.SelectedCells.Count);
                    tempcolstoprint = new SortedList(dgv.SelectedCells.Count);

                    // for each selected cell, add unique rows and columns
                    int colindex, rowindex;
                    foreach (DataGridViewCell cell in dgv.SelectedCells)
                    {
                        colindex = cell.ColumnIndex;
                        rowindex = cell.RowIndex;

                        // add unique rows
                        if (!temprowstoprint.Contains(rowindex))
                        {
                            DataGridViewRow row = dgv.Rows[rowindex];
                            if (row.Visible && !row.IsNewRow)
                                temprowstoprint.Add(rowindex, dgv.Rows[rowindex]);
                        }
                        // add unique columns
                        if (!tempcolstoprint.Contains(colindex))
                            tempcolstoprint.Add(colindex, dgv.Columns[colindex]);
                    }

                    // Move the now-duplicate free columns and rows to our list of what to print
                    rowstoprint = new List<object>(temprowstoprint.Count);
                    foreach (object item in temprowstoprint.Values) rowstoprint.Add(item);
                    colstoprint = new List<object>(tempcolstoprint.Count);
                    foreach (object item in tempcolstoprint.Values) colstoprint.Add(item);
                }
            }
            // if current page was selected, print visible columns for the
            // displayed rows                
            else if (PrintRange.CurrentPage == printRange)
            {
                // create lists
                rowstoprint = new List<object>(dgv.DisplayedRowCount(true));
                colstoprint = new List<object>(dgv.Columns.Count);

                // select all visible rows on displayed page
                for (int i = dgv.FirstDisplayedScrollingRowIndex;
                    i < dgv.FirstDisplayedScrollingRowIndex + dgv.DisplayedRowCount(true);
                    i++)
                {
                    DataGridViewRow row = dgv.Rows[i];
                    if (row.Visible) rowstoprint.Add(row);
                }

                // select all visible columns
                colstoprint = new List<object>(dgv.Columns.Count);
                foreach (DataGridViewColumn col in dgv.Columns) if (col.Visible) colstoprint.Add(col);
            }
            // this is the default for print all - everything marked visible will be printed
            else
            {
                // select all visible rows and all visible columns - but don't include the new 'data entry row' 
                rowstoprint = new List<object>(dgv.Rows.Count);
                foreach (DataGridViewRow row in dgv.Rows) if (row.Visible && !row.IsNewRow) rowstoprint.Add(row);

                colstoprint = new List<object>(dgv.Columns.Count);
                foreach (DataGridViewColumn col in dgv.Columns) if (col.Visible) colstoprint.Add(col);
            }
        }

        /// <summary>
        /// Centralize the string format settings. Build a string format object
        /// using passed in settings, (allowing a user override of a single setting)
        /// and get the alignment from the cell control style.
        /// </summary>
        /// <param name="format">String format, ref parameter with return settings</param>
        /// <param name="controlstyle">DataGridView style to apply (if available)</param>
        /// <param name="alignment">Override text Alignment</param>
        /// <param name="linealignment">Override line alignment</param>
        /// <param name="flags">String format flags</param>
        /// <param name="trim">Override string trimming flags</param>
        /// <returns></returns>
        private void buildstringformat(ref StringFormat format, DataGridViewCellStyle controlstyle,
            StringAlignment alignment, StringAlignment linealignment, StringFormatFlags flags,
            StringTrimming trim)
        {
            // allocate format if it doesn't already exist
            if (null == format)
                format = new StringFormat();

            // Set defaults
            format.Alignment = alignment;
            format.LineAlignment = linealignment;
            format.FormatFlags = flags;
            format.Trimming = trim;

            // Check on right-to-left flag. This is set at the grid level, but doesn't show up 
            // as a cell format. Urgh.
            if ((null != dgv) && (RightToLeft.Yes == dgv.RightToLeft))
                format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

            // use cell alignment to override defaulted alignments
            if (null != controlstyle)
            {
                // Adjust the format based on the control settings, bias towards centered
                DataGridViewContentAlignment cellalign = controlstyle.Alignment;
                if (cellalign.ToString().Contains("Center")) format.Alignment = StringAlignment.Center;
                else if (cellalign.ToString().Contains("Left")) format.Alignment = StringAlignment.Near;
                else if (cellalign.ToString().Contains("Right")) format.Alignment = StringAlignment.Far;

                if (cellalign.ToString().Contains("Top")) format.LineAlignment = StringAlignment.Near;
                else if (cellalign.ToString().Contains("Middle")) format.LineAlignment = StringAlignment.Center;
                else if (cellalign.ToString().Contains("Bottom")) format.LineAlignment = StringAlignment.Far;
            }
        }

        /// <summary>
        /// Calculate cell size based on data versus size settings
        /// </summary>
        /// <param name="g">Current graphics context</param>
        /// <param name="cell">Cell being measured</param>
        /// <param name="index">Column index of cell being measured</param>
        /// <param name="cellstyle">Computed Style of cell being measured</param>
        /// <param name="basewidth">Initial width for size calculation</param>
        /// <param name="format">Computed string format for cell data</param>
        /// <returns>Size of printed cell</returns>
        private SizeF calccellsize(Graphics g, DataGridViewCell cell, int index, DataGridViewCellStyle cellstyle,
            float basewidth, StringFormat format)
        {
            // Start with the grid view cell size
            SizeF size = new SizeF(cell.Size);

            // If we need to do any calculated cell sizes, we need to measure the cell contents
            if ((RowHeightSetting.DataHeight == RowHeight) ||
                (ColumnWidthSetting.DataWidth == ColumnWidth) ||
                (ColumnWidthSetting.Porportional == ColumnWidth))
            {
                SizeF datasize;

                //-------------------------------------------------------------
                // Measure cell contents
                //-------------------------------------------------------------
                if (("DataGridViewImageCell" == dgv.Columns[cell.ColumnIndex].CellType.Name) &&
                    (cell.Value is Image))
                {
                    // measure image if it's an image cell
                    Image img = (System.Drawing.Image)cell.Value;
                    datasize = img.Size;
                }
                else
                {
                    // measure the data for each column, keep widths and biggest height
                    datasize = g.MeasureString(cell.FormattedValue.ToString(), cellstyle.Font,
                        new SizeF(basewidth, maxPages), format);

                    // if we have excessively large cell, limit it to one page width
                    if (printWidth < datasize.Width)
                        datasize = g.MeasureString(cell.FormattedValue.ToString(), cellstyle.Font,
                        new SizeF(pageWidth - cellstyle.Padding.Left - cellstyle.Padding.Right, maxPages),
                        format);
                }

                //-------------------------------------------------------------
                // Add in padding for data based cell sizes and porportional columns
                //-------------------------------------------------------------

                // set cell height to string height if indicated
                if (RowHeightSetting.DataHeight == RowHeight)
                    size.Height = datasize.Height + cellstyle.Padding.Top + cellstyle.Padding.Bottom;

                // set cell width to calculated width if indicated
                if ((ColumnWidthSetting.DataWidth == ColumnWidth) ||
                    (ColumnWidthSetting.Porportional == ColumnWidth))
                    size.Width = datasize.Width + cellstyle.Padding.Left + cellstyle.Padding.Right;
            }

            // final width override - user defined width settings
            if (colwidthsoverride[index] > 0)
                size.Width = colwidthsoverride[index];

            size.Width = size.Width * (float)1.2;//PJB
            return size;
        }

        /// <summary>
        /// Recalculate row heights for cells whose width is greater than the set column width. 
        /// Called when column widths are changed in order to flow text down the page instead of 
        /// accross.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="colindex"></param>
        private void RecalcRowHeights(Graphics g, int colindex, float newcolwidth)
        {
            // search calculated cell sizes for widths larger than our new width
            for (int i = 0; i < rowstoprint.Count; i++)
            {
                if (cellsizes[colindex][i].Width > newcolwidth)
                {
                    StringFormat currentformat = null;

                    // get column style
                    DataGridViewCellStyle colstyle = GetStyle(((DataGridViewRow)rowstoprint[i]), ((DataGridViewColumn)colstoprint[colindex]));

                    // build the cell style and font 
                    buildstringformat(ref currentformat, colstyle, cellformat.Alignment, cellformat.LineAlignment,
                        cellformat.FormatFlags, cellformat.Trimming);

                    // recalculate cell size using new width. This will flow data down the page and 
                    // change the row height
                    SizeF size = calccellsize(g, ((DataGridViewRow)rowstoprint[i]).Cells[colindex], colindex, colstyle,
                        newcolwidth, currentformat);
                    cellsizes[colindex][i] = size;

                    // change the saved row height based on the recalculated size
                    rowheights[i] = (rowheights[i] < size.Height ? size.Height : rowheights[i]);
                }
            }
        }


        /// <summary>
        /// Scan all the rows and columns to be printed and calculate the 
        /// overall individual column width (based on largest column value), 
        /// the header sizes, and determine all the row heights.
        /// </summary>
        /// <param name="g">The graphics context for all measurements</param>
        private void measureprintarea(Graphics g)
        {
            int i, j;
            rowheights = new List<float>(rowstoprint.Count);
            colwidths = new List<float>(colstoprint.Count);
            cellsizes = new List<List<SizeF>>(colstoprint.Count);

            for (i = 0; i < colstoprint.Count; i++)
                cellsizes.Add(new List<SizeF>(rowstoprint.Count));

            footerHeight = 0;

            // temp variables
            DataGridViewColumn col;
            DataGridViewRow row;

            //-----------------------------------------------------------------
            // measure the page headers and footers, including the grid column header cells
            //-----------------------------------------------------------------

            // set initial column sizes based on column titles
            for (i = 0; i < colstoprint.Count; i++)
            {
                col = (DataGridViewColumn)colstoprint[i];

                //-------------------------------------------------------------
                // Build String format and Cell style
                //-------------------------------------------------------------

                // get gridview style, and override if we have a set style for this column
                StringFormat currentformat = null;
                DataGridViewCellStyle headercolstyle = col.HeaderCell.InheritedStyle.Clone();
                if (ColumnHeaderStyles.ContainsKey(col.Name))
                {
                    headercolstyle = columnheaderstyles[col.Name];

                    // build the cell style and font 
                    buildstringformat(ref currentformat, headercolstyle, cellformat.Alignment, cellformat.LineAlignment,
                        cellformat.FormatFlags, cellformat.Trimming);
                }
                else if (col.HasDefaultCellStyle)
                {
                    // build the cell style and font 
                    buildstringformat(ref currentformat, headercolstyle, cellformat.Alignment, cellformat.LineAlignment,
                        cellformat.FormatFlags, cellformat.Trimming);
                }
                else
                {
                    currentformat = columnheadercellformat;
                }

                //-------------------------------------------------------------
                // Calculate and accumulate column header width and height
                //-------------------------------------------------------------

                // deal with overridden col widths
                float usewidth = 0;
                if (0 <= colwidthsoverride[i])
                    usewidth = colwidthsoverride[i];
                else
                    usewidth = printWidth;

                // calculate the size of column header cells
                SizeF size = calccellsize(g, col.HeaderCell, i,
                    headercolstyle, usewidth, columnheadercellformat);

                // save calculated widths
                colwidths.Add(size.Width);

                // accumulate heights, saving largest for data sized option
                if (RowHeightSetting.DataHeight == RowHeight)
                    colheaderheight = (colheaderheight < size.Height ? size.Height : colheaderheight);
                else
                    colheaderheight = size.Height;
            }

            //-----------------------------------------------------------------
            // measure the page number
            //-----------------------------------------------------------------

            if (pageno)
            {
                pagenumberHeight = (g.MeasureString("Page", pagenofont, printWidth, pagenumberformat)).Height;
            }

            //-----------------------------------------------------------------
            // Calc height of header.
            // Header height is height of page number, title, subtitle and height of column headers
            //-----------------------------------------------------------------
            if (PrintHeader)
            {
                // calculate title and subtitle heights
                titleheight = (g.MeasureString(title, titlefont, printWidth, titleformat)).Height;
                subtitleheight = (g.MeasureString(subtitle, subtitlefont, printWidth, subtitleformat)).Height;
            }

            //-----------------------------------------------------------------
            // measure the footer, if one is provided. Include the page number if we're printing
            // it on the bottom
            //-----------------------------------------------------------------
            if (PrintFooter)
            {
                if (!String.IsNullOrEmpty(footer))
                {
                    footerHeight += (g.MeasureString(footer, footerfont, printWidth, footerformat)).Height;
                }

                // note we don't count the page number height if it's not on a separate line
                if (!pagenumberontop && pagenumberonseparateline)
                {
                    footerHeight += pagenumberHeight;
                }

                footerHeight += footerspacing;
            }

            //-----------------------------------------------------------------
            // measure the grid to be printed ... this gets us all the row heights
            // and an accurate measure of column widths for the printed area
            //-----------------------------------------------------------------

            for (i = 0; i < rowstoprint.Count; i++)
            {
                row = (DataGridViewRow)rowstoprint[i];
                rowheights.Add(0);

                // add row headers if they're visible
                if ((bool)PrintRowHeaders)
                {
                    // provide a default 'blank' value to prevent a 0 length if we're supposed to show
                    // row headers
                    String rowheadertext = String.IsNullOrEmpty(row.HeaderCell.FormattedValue.ToString())
                        ? rowheadercelldefaulttext : row.HeaderCell.FormattedValue.ToString();

                    SizeF rhsize = g.MeasureString(rowheadertext,
                        row.HeaderCell.InheritedStyle.Font);
                    rowheaderwidth = (rowheaderwidth < rhsize.Width) ? rhsize.Width + 4 : rowheaderwidth;//EDIT PJB
                }

                // calculate widths for each column. We're looking for the largest width needed for
                // all the rows of data.
                for (j = 0; j < colstoprint.Count; j++)
                {
                    col = (DataGridViewColumn)colstoprint[j];

                    // access the data to be printed 
                    //String datastr = row.Cells[col.Index].FormattedValue.ToString();

                    //-------------------------------------------------------------
                    // Build string format and cell style 
                    //-------------------------------------------------------------

                    // get gridview style, and override if we have a set style for this column
                    StringFormat currentformat = null;
                    DataGridViewCellStyle colstyle = GetStyle(row, col); // = row.Cells[col.Index].InheritedStyle.Clone();

                    // build the cell style and font 
                    buildstringformat(ref currentformat, colstyle, cellformat.Alignment, cellformat.LineAlignment,
                        cellformat.FormatFlags, cellformat.Trimming);

                    //-------------------------------------------------------------
                    // Calculate and accumulate cell widths and heights
                    //-------------------------------------------------------------
                    float basewidth;

                    // get the default width, depending on overrides 
                    if (0 <= colwidthsoverride[j])
                        // set overridden column width
                        basewidth = colwidthsoverride[j];
                    else if (ColumnWidthSetting.CellWidth == ColumnWidth)
                        // set default to same as title cell width
                        basewidth = colwidths[j];
                    else
                        // limit to one page
                        basewidth = printWidth;

                    // remove padding
                    basewidth -= colstyle.Padding.Left + colstyle.Padding.Right;

                    // calc cell size
                    SizeF size = calccellsize(g, row.Cells[col.Index], j, colstyle,
                        basewidth, currentformat);
                    cellsizes[j].Add(size);

                    // save row height and column width
                    if (RowHeightSetting.DataHeight == RowHeight)
                        rowheights[i] = (rowheights[i] < size.Height ? size.Height : rowheights[i]);
                    else
                        rowheights[i] = size.Height;

                    if ((0 <= colwidthsoverride[j]) || ColumnWidthSetting.CellWidth == ColumnWidth)
                        colwidths[j] = size.Width;
                    else
                        colwidths[j] = (colwidths[j] < size.Width ? size.Width : colwidths[j]);
                }
            }

            //-----------------------------------------------------------------
            // Break the columns accross page sets. This is the key to printing
            // where the total width is wider than one page.
            //-----------------------------------------------------------------

            // assume everything will fit on one page
            pagesets = new List<PageDef>();
            pagesets.Add(new PageDef(PrintMargins, colstoprint.Count));
            int pset = 0;

            // Account for row headers 
            pagesets[pset].coltotalwidth = rowheaderwidth;

            // split columns into page sets
            float columnwidth;
            for (i = 0; i < colstoprint.Count; i++)
            {
                // get initial column width
                columnwidth = (colwidthsoverride[i] >= 0)
                    ? colwidthsoverride[i] : colwidths[i];

                // See if the column width takes us off the page - Except for the 
                // first column. This will prevent printing an empty page!! Otherwise,
                // columns longer than the page width are printed on their own page
                if (printWidth < (pagesets[pset].coltotalwidth + columnwidth) && i != 0)
                {
                    pagesets.Add(new PageDef(PrintMargins, colstoprint.Count));
                    pset++;

                    // Account for row headers 
                    pagesets[pset].coltotalwidth = rowheaderwidth;
                }

                // update page set definition 
                pagesets[pset].colstoprint.Add(colstoprint[i]);
                pagesets[pset].colwidths.Add(colwidths[i]);
                pagesets[pset].colwidthsoverride.Add(colwidthsoverride[i]);
                pagesets[pset].coltotalwidth += columnwidth;
            }

            //-----------------------------------------------------------------
            // Adjust column widths and table margins for each page
            //-----------------------------------------------------------------
            for (i = 0; i < pagesets.Count; i++)
                AdjustPageSets(g, pagesets[i]);
        }

        /// <summary>
        /// Adjust column widths for fixed and porportional columns, set the 
        /// margins to enforce the selected tablealignment.
        /// </summary>
        /// <param name="g">The graphics context for all measurements</param>
        /// <param name="pageset">The pageset to adjust</param>
        private void AdjustPageSets(Graphics g, PageDef pageset)
        {
            int i;
            float fixedcolwidth = rowheaderwidth;
            float remainingcolwidth = 0;
            float ratio;

            //-----------------------------------------------------------------
            // Adjust the column widths in the page set to their final values,
            // accounting for overridden widths and porportional column stretching
            //-----------------------------------------------------------------

            // calculate the amount of space reserved for fixed width columns
            for (i = 0; i < pageset.colwidthsoverride.Count; i++)
                if (pageset.colwidthsoverride[i] >= 0)
                    fixedcolwidth += pageset.colwidthsoverride[i];

            // calculate the amount space for non-overridden columns
            for (i = 0; i < pageset.colwidths.Count; i++)
                if (pageset.colwidthsoverride[i] < 0)
                    remainingcolwidth += pageset.colwidths[i];

            // calculate the ratio for porportional columns, use 1 for 
            // non-overridden columns or not porportional
            if ((porportionalcolumns || ColumnWidthSetting.Porportional == ColumnWidth) &&
                0 < remainingcolwidth)
                ratio = ((float)printWidth - fixedcolwidth) / (float)remainingcolwidth;
            else
                ratio = (float)1.0;

            // reset all column widths for override and/or porportionality. coltotalwidth
            // for each pageset should be <= pageWidth
            pageset.coltotalwidth = rowheaderwidth;
            for (i = 0; i < pageset.colwidths.Count; i++)
            {
                if (pageset.colwidthsoverride[i] >= 0)
                    pageset.colwidths[i] = pageset.colwidthsoverride[i];
                else
                {
                    // change the width and recalculate any rows that need to 
                    //flow down the page
                    pageset.colwidths[i] = pageset.colwidths[i] * ratio;
                    RecalcRowHeights(g, i, pageset.colwidths[i]);
                }

                pageset.coltotalwidth += pageset.colwidths[i];
            }

            //-----------------------------------------------------------------
            // Table Alignment - now that we have the column widths established
            // we can reset the table margins to get left, right and centered
            // for the table on the page
            //-----------------------------------------------------------------

            // Reset Print Margins based on table alignment
            if (Alignment.Left == tablealignment)
            {
                // Bias table to the left by setting "right" value
                pageset.margins.Right = pageWidth - pageset.margins.Left - (int)pageset.coltotalwidth;
                if (0 > pageset.margins.Right) pageset.margins.Right = 0;
            }
            else if (Alignment.Right == tablealignment)
            {
                // Bias table to the right by setting "left" value
                pageset.margins.Left = pageWidth - pageset.margins.Right - (int)pageset.coltotalwidth;
                if (0 > pageset.margins.Left) pageset.margins.Left = 0;
            }
            else if (Alignment.Center == tablealignment)
            {
                // Bias the table to the center by setting left and right equal
                pageset.margins.Left = (pageWidth - (int)pageset.coltotalwidth) / 2;
                if (0 > pageset.margins.Left) pageset.margins.Left = 0;
                pageset.margins.Right = pageset.margins.Left;
            }
        }

        /// <summary>
        /// Count the number of pages to print
        /// </summary>
        private int TotalPages()
        {
            float pos = 0;

            // if we're printing by pages, the total pages is the last page to 
            // print
            if (toPage < maxPages)
                return toPage;

            // Start counting pages at 1
            CurrentPage = 1;

            // Calculate where to stop printing the grid - count up from the bottom of the page.
            staticheight = pageHeight - footerHeight - FooterSpacing - PrintMargins.Bottom;

            // Calculate where to start printing the grid for page 1
            pos = PrintMargins.Top + HeaderHeight;

            // if we're printing by rows, sum up rowheights until we're done.
            for (int i = 0; i < (rowheights.Count); i++)
            {
                if (pos + rowheights[i] > staticheight)
                {
                    // count the page
                    CurrentPage++;

                    // reset the counter - depending on setting
                    if (KeepRowsTogether)
                        // if we are keeping rows together, put whole row on next page
                        pos = rowheights[i];
                    else
                        // if we're not keeping rows together, only put remainder on next page
                        pos = pos + rowheights[i] - staticheight;

                    // account for static space at the top of the page
                    pos += PrintMargins.Top + HeaderHeight;
                }
                else
                {
                    // add row space
                    pos += rowheights[i];
                }
            }

            // return counted pages
            return CurrentPage;
        }

        /// <summary>
        /// Check for more pages. This is called at the end of printing a page set.
        /// If there's another page set to print, we return true.
        /// </summary>
        private bool DetermineHasMorePages()
        {
            currentpageset++;
            if (currentpageset < pagesets.Count)
            {
                //currentpageset--;   // decrement back to a valid pageset number
                return true;        // tell the caller we're through.
            }
            else
                return false;
        }

        /// <summary>
        /// This routine prints one page. It will skip non-printable pages if the user 
        /// selected the "some pages" option on the print dialog. This is called during 
        /// the Print event.
        /// </summary>
        /// <param name="g">Graphics object to print to</param>
        private bool PrintPage(Graphics g)
        {
            // flag for continuing or ending print process
            bool HasMorePages = false;

            // flag for handling printing some pages rather than all
            bool printthispage = false;

            // current printing position within one page
            float printpos = pagesets[currentpageset].margins.Top;

            // increment page number & check page range
            CurrentPage++;
            if ((CurrentPage >= fromPage) && (CurrentPage <= toPage))
                printthispage = true;

            // calculate the static vertical space available - this is where we stop printing rows
            staticheight = pageHeight - footerHeight - FooterSpacing - pagesets[currentpageset].margins.Bottom;

            // holder for one-row height lookahead to see if the row will fit on the page
            float nextrowheight;

            //-----------------------------------------------------------------
            // scan down heights until we're off this (non-printing) page
            //-----------------------------------------------------------------

            while (!printthispage)
            {
                // calculate and increment over the page we're not printing
                printpos = pagesets[currentpageset].margins.Top + HeaderHeight;

                // are we done with this page?
                bool pagecomplete = false;
                currentrow = lastrowprinted + 1;

                // get height of first row on page
                nextrowheight = (lastrowprinted < rowheights.Count) ? rowheights[currentrow] : 0;
                do
                {
                    // this is how much space this row will use on this page
                    float used = (rowheights[currentrow] - rowstartlocation) > (staticheight - printpos)
                        ? (staticheight - printpos) : rowheights[currentrow] - rowstartlocation;
                    printpos += used;

                    // Now, start checking on whether or not we're out of room & need to count a page

                    // do we have more of this row to print?
                    if ((rowstartlocation + used) >= nextrowheight)
                    {
                        // completed a row, so reset startlocation and count this row.
                        rowstartlocation = 0;
                        lastrowprinted++;
                        currentrow++;
                    }
                    else
                    {
                        // more on this row to print, but we're out of space on this page
                        rowstartlocation += used;
                        pagecomplete = true;
                    }

                    // check to see if we should break before printing the next row,
                    // i.e. we're at the top of a new row and we're keeping rows together, then check to 
                    // see if there's enough room.

                    // do one row look-ahead to see if we have room on the page
                    nextrowheight = (currentrow < rowheights.Count) ? rowheights[currentrow] : 0;

                    if ((0 == rowstartlocation) && keepRowsTogether)
                    {
                        // check to see if we have room on the page to print the whole row
                        if ((printpos + nextrowheight) >= staticheight)
                        {
                            pagecomplete = true;
                        }
                    }

                    // If we're off the page, then stop  
                    if ((0 == rowstartlocation) && printpos >= staticheight)
                        pagecomplete = true;

                    // if we're out of data (no partial rows and no more rows)
                    if ((0 == rowstartlocation) && lastrowprinted >= rowstoprint.Count - 1)
                        pagecomplete = true;

                } while (!pagecomplete);

                // skip to the next page & see if it's in the print range
                CurrentPage++;

                if ((CurrentPage >= fromPage) && (CurrentPage <= toPage))
                    printthispage = true;

                // partial row means more to print
                if (0 != rowstartlocation)
                {
                    // we're not done with this row yet
                    HasMorePages = true;
                }
                // done with this page set so see if there are any more pagesets to print
                else if ((lastrowprinted >= rowstoprint.Count - 1) || (CurrentPage > toPage))
                {
                    // reset for next pageset or tell the caller we're complete
                    HasMorePages = DetermineHasMorePages();

                    // reset counters since we'll go through this twice if we print from preview
                    lastrowprinted = -1;
                    CurrentPage = 0;

                    return HasMorePages;
                }
            }

            //-----------------------------------------------------------------
            // print headers
            //-----------------------------------------------------------------

            // reset printpos as it may have changed during the 'skip pages' routine just above.
            printpos = pagesets[currentpageset].margins.Top;

            // Skip headers if the flag is false
            if (PrintHeader)
            {
                // print any "header" images so that anything else we print will be 'on top'
                ImbeddedImageList.Where(p => p.ImageLocation == Location.Header).DrawImbeddedImage(g, printWidth,
                    pageHeight, pagesets[currentpageset].margins);

                // print page number if user selected it
                if (pagenumberontop)
                {
                    // if we have a page number to print
                    if (pageno)
                    {
                        String pagenumber = pagetext + CurrentPage.ToString(CultureInfo.CurrentCulture);
                        if (showtotalpagenumber)
                        {
                            pagenumber += pageseparator + totalpages.ToString();
                        }
                        if (1 < pagesets.Count)
                            pagenumber += parttext + (currentpageset + 1).ToString(CultureInfo.CurrentCulture);

                        // ... then print it
                        printsection(g, ref printpos,
                            pagenumber, pagenofont, pagenocolor, pagenumberformat,
                            overridepagenumberformat, pagesets[currentpageset].margins);

                        // if the page number is not on a separate line, don't "use up" it's vertical space
                        if (!pagenumberonseparateline)
                            printpos -= pagenumberHeight;
                    }
                }

                // print title if provided, & we're not skipping it
                if (0 != TitleHeight && !String.IsNullOrEmpty(title))
                    printsection(g, ref printpos, title, titlefont,
                        titlecolor, titleformat, overridetitleformat,
                        pagesets[currentpageset].margins);

                // account for title spacing
                printpos += TitleHeight;

                // print subtitle if provided
                if (0 != SubTitleHeight && !String.IsNullOrEmpty(subtitle))
                    printsection(g, ref printpos, subtitle, subtitlefont,
                        subtitlecolor, subtitleformat, overridesubtitleformat,
                        pagesets[currentpageset].margins);

                // account for subtitle spacing
                printpos += SubTitleHeight;
            }

            // print the column headers or not based on our processing flag
            if ((bool)PrintColumnHeaders)
            {
                // print column headers
                printcolumnheaders(g, ref printpos, pagesets[currentpageset]);
            }

            //-----------------------------------------------------------------
            // print statically located images
            //-----------------------------------------------------------------

            // print any "absolute" images so that anything else we print will be 'on top'
            ImbeddedImageList.Where(p => p.ImageLocation == Location.Absolute).DrawImbeddedImage(g, printWidth,
                pageHeight, pagesets[currentpageset].margins);

            //-----------------------------------------------------------------
            // print rows until the page is complete
            //-----------------------------------------------------------------
            bool continueprinting = true;
            currentrow = lastrowprinted + 1;

            if (currentrow >= rowstoprint.Count)
            {
                // indicate that we're done printing - shouldn't ever this this
                return false;
            }

            // get the height of the first row to print
            nextrowheight = (lastrowprinted < rowheights.Count) ? rowheights[currentrow] : 0;
            do
            {
                // print the part of the row that we can, and accumulate the space used
                float used = printrow(g, printpos, (DataGridViewRow)(rowstoprint[currentrow]),
                    pagesets[currentpageset], rowstartlocation);
                printpos += used;

                // Now, start checking on whether or not to print the next row 
                // (or if we even have a next row)

                // do we have more of this row to print?
                if ((rowstartlocation + used) >= nextrowheight)
                {
                    // completed a row, so reset startlocation and count this row.
                    rowstartlocation = 0;
                    lastrowprinted++;
                    currentrow++;
                }
                else
                {
                    // more on this row to print, but we're out of space on this page
                    rowstartlocation += used;
                    continueprinting = false;
                }

                // check to see if we should break before printing the next row,
                // i.e. we're at the top of a new row and we're keeping rows together, then check to 
                // see if there's enough room.

                // do one row look-ahead to see if we have room on the page
                nextrowheight = (currentrow < rowheights.Count) ? rowheights[currentrow] : 0;

                if ((0 == rowstartlocation) && keepRowsTogether)
                {
                    // check to see if we have room on the page to print the whole row
                    if ((printpos + nextrowheight) >= staticheight)
                    {
                        continueprinting = false;
                    }
                }

                // If we're off the page, then stop  
                if ((0 == rowstartlocation) && printpos >= staticheight)
                    continueprinting = false;

                // if we're out of data (no partial rows and no more rows)
                if ((0 == rowstartlocation) && lastrowprinted >= rowstoprint.Count - 1)
                    continueprinting = false;

            } while (continueprinting);

            //-----------------------------------------------------------------
            // print footer
            //-----------------------------------------------------------------
            if (PrintFooter)
            {
                // print any "footer" images so that anything else we print will be 'on top'
                ImbeddedImageList.Where(p => p.ImageLocation == Location.Footer).DrawImbeddedImage(g, printWidth,
                    pageHeight, pagesets[currentpageset].margins);

                printfooter(g, ref printpos, pagesets[currentpageset].margins);
            }

            //-----------------------------------------------------------------
            // bottom check, see if this is the last page to print
            //-----------------------------------------------------------------

            // partial row means more to print
            if (0 != rowstartlocation)
            {
                // we're not done with this row yet
                HasMorePages = true;
            }

            // done with this page set so see if there are any more pagesets to print
            if ((CurrentPage >= toPage) || (lastrowprinted >= rowstoprint.Count - 1))
            {
                // reset for next pageset or tell the caller we're complete
                HasMorePages = DetermineHasMorePages();

                // reset counters since we'll go through this twice if we print from preview
                rowstartlocation = 0;
                lastrowprinted = -1;
                CurrentPage = 0;
            }
            else
            {
                // we're not done yet
                HasMorePages = true;
            }

            return HasMorePages;
        }

        /// <summary>
        /// Print a header or footer section. Used for page numbers and titles
        /// </summary>
        /// <param name="g">Graphic context to print in</param>
        /// <param name="pos">Track vertical space used; 'y' location</param>
        /// <param name="text">String to print</param>
        /// <param name="font">Font to use for printing</param>
        /// <param name="color">Color to print in</param>
        /// <param name="format">String format for text</param>
        /// <param name="useroverride">True if the user overrode the alignment or flags</param>
        /// <param name="margins">The table's print margins</param>
        private void printsection(Graphics g, ref float pos, string text,
            Font font, Color color, StringFormat format, bool useroverride, Margins margins)
        {
            // measure string
            SizeF printsize = g.MeasureString(text, font, printWidth, format);

            // build area to print within
            RectangleF printarea = new RectangleF((float)margins.Left, pos, (float)printWidth,
               printsize.Height);

            // do the actual print
            g.DrawString(text, font, new SolidBrush(color), printarea, format);

            // track "used" vertical space
            pos += printsize.Height;
        }

        /// <summary>
        /// Print the footer. This handles the footer spacing, and printing the page number
        /// at the bottom of the page (if the page number is not in the header).
        /// </summary>
        /// <param name="g">Graphic context to print in</param>
        /// <param name="pos">Track vertical space used; 'y' location</param>
        /// <param name="margins">The table's print margins</param>
        private void printfooter(Graphics g, ref float pos, Margins margins)
        {
            // print last footer. Note: need to force printpos to the bottom of the page
            // as we may have run out of data anywhere on the page
            pos = pageHeight - footerHeight - margins.Bottom;  // - margins.Top

            // add spacing
            pos += footerspacing;

            // print the footer
            printsection(g, ref pos, footer, footerfont, footercolor, footerformat,
                overridefooterformat, margins);

            // print the page number if it's on the bottom.
            if (!pagenumberontop)
            {
                if (pageno)
                {
                    String pagenumber = pagetext + CurrentPage.ToString(CultureInfo.CurrentCulture);
                    if (showtotalpagenumber)
                    {
                        pagenumber += pageseparator + totalpages.ToString();
                    }
                    if (1 < pagesets.Count)
                        pagenumber += parttext + (currentpageset + 1).ToString(CultureInfo.CurrentCulture);

                    // if the pageno is not on a separate line, push the print location up by its height.
                    if (!pagenumberonseparateline)
                        pos = pos - pagenumberHeight;

                    // print the page number
                    printsection(g, ref pos, pagenumber, pagenofont, pagenocolor, pagenumberformat,
                        overridepagenumberformat, margins);
                }
            }
        }

        /// <summary>
        /// Print the column headers. Most printing format info is retrieved from the 
        /// source DataGridView.
        /// </summary>
        /// <param name="g">Graphics Context to print within</param>
        /// <param name="pos">Track vertical space used; 'y' location</param>
        /// <param name="pageset">Current pageset - defines columns and margins</param>
        private void printcolumnheaders(Graphics g, ref float pos, PageDef pageset)
        {
            // track printing location accross the page. start position is hard left,
            // adjusted for the row headers. Note rowheaderwidth is 0 if row headers are not printed
            float xcoord = pageset.margins.Left + rowheaderwidth;

            // set the pen for drawing the grid lines
            Pen lines = new Pen(dgv.GridColor, 1);

            //-----------------------------------------------------------------
            // Print the column headers
            //-----------------------------------------------------------------
            DataGridViewColumn col;
            for (int i = 0; i < pageset.colstoprint.Count; i++)
            {
                col = (DataGridViewColumn)pageset.colstoprint[i];

                // calc cell width, account for columns larger than the print area!
                float cellwidth = (pageset.colwidths[i] > printWidth - rowheaderwidth ?
                    printWidth - rowheaderwidth : pageset.colwidths[i]);

                // get column style
                DataGridViewCellStyle style = col.HeaderCell.InheritedStyle.Clone();
                if (ColumnHeaderStyles.ContainsKey(col.Name))
                {
                    style = ColumnHeaderStyles[col.Name];
                }

                // set print area for this individual cell, account for cells larger
                // than the print area!
                RectangleF cellprintarea = new RectangleF(xcoord, pos, cellwidth, colheaderheight);

                DrawCell(g, cellprintarea, style, col.HeaderCell, 0, columnheadercellformat, lines);

                xcoord += pageset.colwidths[i];
            }

            // all done, consume "used" vertical space, including space for border lines
            pos += colheaderheight +
                (dgv.ColumnHeadersBorderStyle != DataGridViewHeaderBorderStyle.None ? lines.Width : 0);
        }

        /// <summary>
        /// Print one row of the DataGridView. Most printing format info is retrieved
        /// from the DataGridView.
        /// </summary>
        /// <param name="g">Graphics Context to print within</param>
        /// <param name="pos">Track vertical space used; 'y' location</param>
        /// <param name="row">The row that will be printed</param>
        /// <param name="pageset">Current Pageset - defines columns and margins</param>
        /// <param name="startline">Line no. in row to start printing text at</param>
        private float printrow(Graphics g, float finalpos, DataGridViewRow row, PageDef pageset,
            float startlocation)
        {
            // track printing location accross the page
            float xcoord = pageset.margins.Left;
            float pos = finalpos;

            // set the pen for drawing the grid lines
            Pen lines = new Pen(dgv.GridColor, 1);

            // calc row width, account for columns wider than the print area!
            float rowwidth = (pageset.coltotalwidth > printWidth ? printWidth : pageset.coltotalwidth);

            // calc row heigth in pixels to print
            float rowheight = (rowheights[currentrow] - startlocation) > (staticheight - pos)
                ? (staticheight - pos) : rowheights[currentrow] - startlocation;

            //-----------------------------------------------------------------
            // Print Row background
            //-----------------------------------------------------------------

            // get current row style, start with header style
            DataGridViewCellStyle rowstyle = row.InheritedStyle.Clone();

            // define print rectangle
            RectangleF printarea = new RectangleF(xcoord, pos, rowwidth,
                rowheight);

            // fill in the row background as the default color
            g.FillRectangle(new SolidBrush(rowstyle.BackColor), printarea);

            //-----------------------------------------------------------------
            // Print the Row Headers, if they are visible
            //-----------------------------------------------------------------
            if ((bool)PrintRowHeaders)
            {
                // set print area for this individual cell
                RectangleF headercellprintarea = new RectangleF(xcoord, pos,
                    rowheaderwidth, rowheight);

                DrawCell(g, headercellprintarea, rowstyle, row.HeaderCell, startlocation,
                    rowheadercellformat, lines);

                // track horizontal space used
                xcoord += rowheaderwidth;
            }

            //-----------------------------------------------------------------
            // Print the row: write and draw each cell
            //-----------------------------------------------------------------
            DataGridViewColumn col;
            for (int i = 0; i < pageset.colstoprint.Count; i++)
            {
                // access the cell and column being printed
                col = (DataGridViewColumn)pageset.colstoprint[i];
                DataGridViewCell cell = row.Cells[col.Index];

                // calc cell width, account for columns larger than the print area!
                float cellwidth = (pageset.colwidths[i] > printWidth - rowheaderwidth ?
                    printWidth - rowheaderwidth : pageset.colwidths[i]);

                // SLG 01112010 - only draw columns with an actual width
                if (cellwidth > 0)
                {
                    // get DGV column style and see if we have an override for this column
                    StringFormat finalformat = null;
                    Font cellfont = null;
                    DataGridViewCellStyle colstyle = GetStyle(row, col); // = row.Cells[col.Index].InheritedStyle.Clone(); 
                                                                         //if (ColumnStyles.ContainsKey(col.Name))
                                                                         //{
                                                                         //    colstyle = colstyles[col.Name];

                    // set string format
                    buildstringformat(ref finalformat, colstyle, cellformat.Alignment, cellformat.LineAlignment,
                        cellformat.FormatFlags, cellformat.Trimming);
                    cellfont = colstyle.Font;
                    //}
                    //else if ((col.HasDefaultCellStyle) || (row.Cells[col.Index].HasStyle))
                    //{
                    //    // set string format
                    //    buildstringformat(ref finalformat, colstyle, cellformat.Alignment, cellformat.LineAlignment,
                    //        cellformat.FormatFlags, cellformat.Trimming);
                    //    cellfont = colstyle.Font;
                    //}
                    //else
                    //{
                    //    finalformat = cellformat;
                    //}

                    // set overall print area for this individual cell 
                    RectangleF cellprintarea = new RectangleF(xcoord, pos, cellwidth,
                        rowheight);

                    DrawCell(g, cellprintarea, colstyle, cell, startlocation, finalformat, lines);
                }
                // track horizontal space used
                xcoord += pageset.colwidths[i];
            }

            //-----------------------------------------------------------------
            // All done with this row, consume "used" vertical space
            //-----------------------------------------------------------------
            return rowheight;
        }

        /// <summary>
        /// Allow override of cell drawing. This is to support grids that have onPaint
        /// overridden to do things like images in header rows and vertical printing
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rowindex"></param>
        /// <param name="columnindex"></param>
        /// <param name="rectf"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        Boolean DrawOwnerDrawCell(Graphics g, int rowindex, int columnindex, RectangleF rectf,
            DataGridViewCellStyle style)
        {
            DGVCellDrawingEventArgs args = new DGVCellDrawingEventArgs(g, rectf, style,
                rowindex, columnindex);
            OnCellOwnerDraw(args);
            return args.Handled;
        }

        /// <summary>
        /// Draw a cell. Used for column and row headers and body cells.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="cellprintarea"></param>
        /// <param name="style"></param>
        /// <param name="cell"></param>
        /// <param name="startlocation"></param>
        /// <param name="cellformat"></param>
        /// <param name="lines"></param>
        void DrawCell(Graphics g, RectangleF cellprintarea, DataGridViewCellStyle style,
            DataGridViewCell cell, float startlocation, StringFormat cellformat, Pen lines)
        {
            // Draw the cell if it's not overridden by ownerdrawing
            if (!DrawOwnerDrawCell(g, cell.RowIndex, cell.ColumnIndex, cellprintarea, style))
            {
                // save original clipping bounds
                RectangleF clip = g.ClipBounds;

                // fill in the full cell background - using the selected style
                //g.FillRectangle(new SolidBrush(colstyle.BackColor), cellprintarea);
                g.FillRectangle(new SolidBrush(style.BackColor), cellprintarea);

                // reset print area for this individual cell, adjusting 'inward' for cell padding
                cellprintarea = new RectangleF(cellprintarea.X + style.Padding.Left,
                    cellprintarea.Y + style.Padding.Top,
                    cellprintarea.Width - style.Padding.Right - style.Padding.Left,
                    cellprintarea.Height - style.Padding.Bottom - style.Padding.Top);

                // set clipping to current print area - i.e. our cell
                g.SetClip(cellprintarea);

                // define the *actual* print area based on the given startlocation. Offset the start by 
                // minus the start location, increase the print area height by the startlocation
                RectangleF actualprint = new RectangleF(cellprintarea.X, cellprintarea.Y - startlocation,
                    cellprintarea.Width, cellprintarea.Height + startlocation);

                // draw content based on cell style, but only for "body" cells
                if (0 <= cell.RowIndex && 0 <= cell.ColumnIndex &&
                    ("DataGridViewImageCell" == dgv.Columns[cell.ColumnIndex].CellType.Name))
                {
                    DrawImageCell(g, (DataGridViewImageCell)cell, actualprint);
                }
                else
                {
                    // draw the text for the cell at the row / col intersection
                    g.DrawString(cell.FormattedValue.ToString(), style.Font,
                        new SolidBrush(style.ForeColor), actualprint, cellformat);
                }

                // reset clipping bounds to "normal"
                g.SetClip(clip);

                // draw the borders - default to the dgv's border setting
                if (dgv.CellBorderStyle != DataGridViewCellBorderStyle.None)
                    g.DrawRectangle(lines, cellprintarea.X, cellprintarea.Y, cellprintarea.Width, cellprintarea.Height);
            }
        }

        /// <summary>
        /// Draw a body cell that has an imbedded image
        /// </summary>
        /// <param name="g"></param>
        /// <param name="imagecell"></param>
        /// <param name="rectf"></param>
        void DrawImageCell(Graphics g, DataGridViewImageCell imagecell, RectangleF rectf)
        {
            // image to draw
            Image img = (System.Drawing.Image)imagecell.Value;

            // clipping bounds. This is the portion of the image to fit into the drawing rectangle
            Rectangle src = new Rectangle();

            // calculate deltas
            int dx = 0;
            int dy = 0;

            // drawn normal size, clipped to cell 
            if ((DataGridViewImageCellLayout.Normal == imagecell.ImageLayout) ||
                (DataGridViewImageCellLayout.NotSet == imagecell.ImageLayout))
            {
                // calculate origin deltas, used to move image
                dx = img.Width - (int)rectf.Width;
                dy = img.Height - (int)rectf.Height;

                // set destination width and height to clip to cell
                if (0 > dx) rectf.Width = src.Width = img.Width; else src.Width = (int)rectf.Width;
                if (0 > dy) rectf.Height = src.Height = img.Height; else src.Height = (int)rectf.Height;

            }
            else if (DataGridViewImageCellLayout.Stretch == imagecell.ImageLayout)
            {
                // stretch image to fit cell size
                src.Width = img.Width;
                src.Height = img.Height;

                // change the origin delta's to 0 so we don't move the image
                dx = 0;
                dy = 0;
            }
            else // DataGridViewImageCellLayout.Zoom
            {
                // scale image to fit in cell
                src.Width = img.Width;
                src.Height = img.Height;

                float vertscale = rectf.Height / src.Height;
                float horzscale = rectf.Width / src.Width;
                float scale;

                // use the smaller scaling factor to ensure the image will fit in the cell
                if (vertscale > horzscale)
                {
                    // use horizontal scale, don't move image horizontally
                    scale = horzscale;
                    dx = 0;
                    dy = (int)((src.Height * scale) - rectf.Height);
                }
                else
                {
                    // use vertical scale, don't move image vertically
                    scale = vertscale;
                    dy = 0;
                    dx = (int)((src.Width * scale) - rectf.Width);
                }

                // set target size to match scaled image
                rectf.Width = src.Width * scale;
                rectf.Height = src.Height * scale;
            }

            //calculate image drawing origin based on origin deltas
            switch (imagecell.InheritedStyle.Alignment)
            {
                case DataGridViewContentAlignment.BottomCenter:
                    if (0 > dy) rectf.Y -= dy; else src.Y = dy;
                    if (0 > dx) rectf.X -= dx / 2; else src.X = dx / 2;
                    break;
                case DataGridViewContentAlignment.BottomLeft:
                    if (0 > dy) rectf.Y -= dy; else src.Y = dy;
                    src.X = 0;
                    break;
                case DataGridViewContentAlignment.BottomRight:
                    if (0 > dy) rectf.Y -= dy; else src.Y = dy;
                    if (0 > dx) rectf.X -= dx; else src.X = dx;
                    break;
                case DataGridViewContentAlignment.MiddleCenter:
                    if (0 > dy) rectf.Y -= dy / 2; else src.Y = dy / 2;
                    if (0 > dx) rectf.X -= dx / 2; else src.X = dx / 2;
                    break;
                case DataGridViewContentAlignment.MiddleLeft:
                    if (0 > dy) rectf.Y -= dy / 2; else src.Y = dy / 2;
                    src.X = 0;
                    break;
                case DataGridViewContentAlignment.MiddleRight:
                    if (0 > dy) rectf.Y -= dy / 2; else src.Y = dy / 2;
                    if (0 > dx) rectf.X -= dx; else src.X = dx;
                    break;
                case DataGridViewContentAlignment.TopCenter:
                    src.Y = 0;
                    if (0 > dx) rectf.X -= dx / 2; else src.X = dx / 2;
                    break;
                case DataGridViewContentAlignment.TopLeft:
                    src.Y = 0;
                    src.X = 0;
                    break;
                case DataGridViewContentAlignment.TopRight:
                    src.Y = 0;
                    if (0 > dx) rectf.X -= dx; else src.X = dx;
                    break;
                case DataGridViewContentAlignment.NotSet:
                    if (0 > dy) rectf.Y -= dy / 2; else src.Y = dy / 2;
                    if (0 > dx) rectf.X -= dx / 2; else src.X = dx / 2;
                    break;
            }

            // Now we can draw our image
            g.DrawImage(img, rectf, src, GraphicsUnit.Pixel);
        }
    }
}


