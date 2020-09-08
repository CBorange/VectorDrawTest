using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdCommandLine;
using VectorDraw.Professional.Control;
using VectorDraw.Professional.vdPrimaries;
using VectorDraw.Professional.vdFigures;

namespace Hicom.BizDraw.DrawControls
{
    class BizDrawContextMenuHandler
    {
        #region Default MenuItem Key
        private const string MENU_UNDO = "UNDO";
        private const string MENU_REDO = "REDO";
        private const string MENU_ZOOM = "ZOOM";
        private const string MENU_ZOOM_ALL = "ZOOM_ALL";
        private const string MENU_ZOOM_EXTENDS = "ZOOM_EXTENDS";
        private const string MENU_ZOOM_PREVIOUS = "ZOOM_PREVIOUS";
        private const string MENU_ZOOM_WINDOW = "ZOOM_WINDOW";
        private const string MENU_ZOOM_IN = "ZOOM_IN";
        private const string MENU_ZOOM_OUT = "ZOOM_OUT";
        private const string MENU_PAN = "PAN";
        private const string MENU_REDRAW = "REDRAW";
        private const string MENU_DELETE = "DELETE";
        private const string MENU_MOVE = "MOVE";
        private const string MENU_COPY = "COPY";
        private const string MENU_ROTATE = "ROTATE";
        private const string MENU_PROPERTY = "PROPERTY";
        private const string MENU_EXPORT = "EXPORT";
        private const string MENU_DIM_OPTIONS = "DIM_OPTIONS";
        private const string MENU_OSNAP = "MENU_OSNAP";
        #endregion

        private vdDocument Document { get; set; }
        private vdCommandLine CommandLine { get; set; }
        private Control ParentControl { get; set; }
        public ContextMenu BizDrawContextMenu { get; private set; }

        public BizDrawContextMenuHandler(Control parentControl, vdCommandLine commandLine, vdDocument doc)
        {
            this.Document = doc;
            this.CommandLine = commandLine;
            this.ParentControl = parentControl;
            this.InitializeComponent();
        }

        public void InitializeComponent()
        {
            BizDrawContextMenu = new ContextMenu();

            //this.AddMenuItem(string.Empty, MENU_UNDO, "Undo");
            //this.AddMenuItem(string.Empty, MENU_REDO, "Redo");
            this.AddMenuItem(string.Empty, MENU_ZOOM, "Zoom");
            this.AddMenuItem(MENU_ZOOM, MENU_ZOOM_ALL, "Zoom All");
            this.AddMenuItem(MENU_ZOOM, MENU_ZOOM_EXTENDS, "Zoom Extends");
            this.AddMenuItem(MENU_ZOOM, MENU_ZOOM_PREVIOUS, "Zoom Previous");
            this.AddMenuItem(MENU_ZOOM, MENU_ZOOM_WINDOW, "Zoom Window");
            this.AddMenuItem(MENU_ZOOM, MENU_ZOOM_IN, "Zoom In");
            this.AddMenuItem(MENU_ZOOM, MENU_ZOOM_OUT, "Zoom Out");
            this.AddMenuItem(string.Empty, MENU_PAN, "Pan");
            this.AddMenuItem(string.Empty, MENU_OSNAP, "OSnap");
            this.AddMenuItem(string.Empty, MENU_REDRAW, "Redraw");

            this.AddMenuItem(string.Empty, MENU_DELETE, "Delete");
            this.AddMenuItem(string.Empty, MENU_MOVE, "Move");
            this.AddMenuItem(string.Empty, MENU_COPY, "Copy");
            this.AddMenuItem(string.Empty, MENU_ROTATE, "Rotate");
            this.AddMenuItem(string.Empty, MENU_PROPERTY, "Property");
            
            this.AddMenuItem(string.Empty, MENU_EXPORT, "Export");

            // 기존에는 치수선에 개별 옵션을 주었지만 이건 백터드로우 고유 기능이며
            // 오토캐드에서는 지원이 되는게 아님
            // 그래서 이 옵션 창은 더 이상 사용 못함
            //this.AddMenuItem(string.Empty, MENU_DIM_OPTIONS, "Dimension Options");

            BizDrawContextMenu.Popup += new EventHandler(BizDrawContextMenu_Popup);
        }

        private void BizDrawContextMenu_Popup(object sender, EventArgs e)
        {
            bool isSelection = false;
            
            vdSelection selection = this.GetSelectionEntities();
            if (selection != null && selection.Count > 0)
                isSelection = true;

            //this.FindMenuItem(MENU_UNDO).Visible = !isSelection;
            //this.FindMenuItem(MENU_REDO).Visible = !isSelection;
            this.FindMenuItem(MENU_ZOOM).Visible = !isSelection;
            this.FindMenuItem(MENU_ZOOM_ALL).Visible = !isSelection;
            this.FindMenuItem(MENU_ZOOM_EXTENDS).Visible = !isSelection;
            this.FindMenuItem(MENU_ZOOM_PREVIOUS).Visible = !isSelection;
            this.FindMenuItem(MENU_ZOOM_WINDOW).Visible = !isSelection;
            this.FindMenuItem(MENU_ZOOM_IN).Visible = !isSelection;
            this.FindMenuItem(MENU_ZOOM_OUT).Visible = !isSelection;
            this.FindMenuItem(MENU_PAN).Visible = !isSelection;
            this.FindMenuItem(MENU_OSNAP).Visible = true;
            this.FindMenuItem(MENU_REDRAW).Visible = !isSelection;
            
            this.FindMenuItem(MENU_DELETE).Visible = isSelection;
            this.FindMenuItem(MENU_MOVE).Visible = isSelection;
            this.FindMenuItem(MENU_COPY).Visible = isSelection;
            this.FindMenuItem(MENU_ROTATE).Visible = isSelection;
            this.FindMenuItem(MENU_PROPERTY).Visible = isSelection;
            //this.FindMenuItem(MENU_DIM_OPTIONS).Visible = true;

            if (isSelection && ParentControl.FindForm() is EntityImportForm)
                this.FindMenuItem(MENU_EXPORT).Visible = isSelection;
            else
                this.FindMenuItem(MENU_EXPORT).Visible = false;
        }

        private vdSelection GetSelectionEntities()
        {
            StringBuilder sbSelectionName = new StringBuilder("VDGRIPSET_");
            sbSelectionName.Append(this.Document.ActiveLayOut.Handle.ToStringValue());
            if (this.Document.ActiveLayOut.ActiveViewPort != null)
                sbSelectionName.Append(this.Document.ActiveLayOut.ActiveViewPort.Handle.ToString());

            vdSelection selection = this.Document.Selections.FindName(sbSelectionName.ToString());
            
            return selection;
        }
        
        public MenuItem FindMenuItem(string menuItemName)
        {
            if (!string.IsNullOrEmpty(menuItemName))
            {
                MenuItem[] menuItemArray = BizDrawContextMenu.MenuItems.Find(menuItemName, true);
                if (menuItemArray != null && menuItemArray.Length > 0)
                    return menuItemArray[0];
            }
            
            return null;
        }
        
        public bool AddMenuItem(string parentMenuItemName, string menuItemName, string menuItemCaption, EventHandler menuItemClickHandler = null)
        {
            bool result = false;

            if (this.FindMenuItem(menuItemName) != null)
                return result;
            
            MenuItem menuItem = new MenuItem();
            menuItem.Name = menuItemName;
            menuItem.Text = menuItemCaption;
            if (menuItemClickHandler == null)
                menuItem.Click += new EventHandler(MenuItem_Click);
            else
                menuItem.Click += menuItemClickHandler;

            MenuItem parentMenuItem = FindMenuItem(parentMenuItemName);
            if (parentMenuItem != null)
                result = parentMenuItem.MenuItems.Add(menuItem) > 0 ? true : false;
            else
                result = BizDrawContextMenu.MenuItems.Add(menuItem) > 0 ? true : false;
            
            return result;
        }
        
        public void DelMenuItem(string parentMenuItemName, string menuItemName)
        {
            if (string.IsNullOrEmpty(parentMenuItemName))
            {
                MenuItem parentMenuItem = this.FindMenuItem(parentMenuItemName);
                if (parentMenuItemName != null)
                {
                    MenuItem menuItem = this.FindMenuItem(menuItemName);
                    parentMenuItem.MenuItems.Remove(menuItem);
                }
            }
            else
            {
                MenuItem menuItem = this.FindMenuItem(menuItemName);
                BizDrawContextMenu.MenuItems.Remove(menuItem); 
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem menuItem = sender as MenuItem;
                switch (menuItem.Name)
                {
                    case MENU_UNDO:
                        Document.UndoHistory.Undo();
                        break;
                    case MENU_REDO:
                        Document.UndoHistory.Redo();
                        break;
                    case MENU_ZOOM_ALL:
                        this.CommandLine.ExecuteCommand(Command.Command.ZOOM);
                        break;
                    case MENU_ZOOM_EXTENDS:
                        this.Document.ZoomExtents();
                        break;
                    case MENU_ZOOM_PREVIOUS:
                        this.Document.ZoomPrevious();
                        break;
                    case MENU_ZOOM_WINDOW:
                        this.CommandLine.ExecuteCommand(Command.Command.ZOOM_WINDOW);
                        break;
                    case MENU_ZOOM_IN:
                        this.CommandLine.ExecuteCommand(Command.Command.ZOOM_IN);
                        break;
                    case MENU_ZOOM_OUT:
                        this.CommandLine.ExecuteCommand(Command.Command.ZOOM_OUT);
                        break;
                    //case MENU_PAN:
                    //    break;
                    case MENU_REDRAW:
                        this.CommandLine.ExecuteCommand(Command.Command.REDRAW);
                        break;
                    case MENU_DELETE:
                        this.CommandLine.ExecuteCommand(Command.Command.DELETE);
                        break;
                    case MENU_MOVE:
                        this.CommandLine.ExecuteCommand(Command.Command.MOVE);
                        break;
                    case MENU_COPY:
                        this.CommandLine.ExecuteCommand(Command.Command.COPY);
                        break;
                    case MENU_ROTATE:
                        this.CommandLine.ExecuteCommand(Command.Command.ROTATE);
                        break;
                    case MENU_PROPERTY:
                        this.CommandLine.ExecuteCommand(Command.Command.PROPERTY);
                        break;
                    case MENU_EXPORT:
                        this.CommandLine.ExecuteCommand(Command.Command.EXPORT);
                        break;
                    case MENU_DIM_OPTIONS:
                        this.CommandLine.ExecuteCommand(Command.Command.DIM_OPTIONS);
                        break;
                    case MENU_OSNAP:
                        this.CommandLine.ExecuteCommand(Command.Command.OSNAP_WINDOW);
                        break;
                    //default:
                    //    break;
                }
            }
        }
    }
}
