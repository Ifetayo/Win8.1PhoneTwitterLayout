using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace WinPhone8._1TwitterCustomPanel.ControlPanel
{
    /// <summary>
    /// This class is responsible for providing a custom control layout 
    /// for the items that act as navigation headers for the listbox.
    /// </summary>
    /// <remarks>
    /// The name of this custom control is SplitPanel (class name)
    /// This class specifies the layout template for the control it is attached
    /// to. Basically gives us full control as to the size interms of height and
    /// width of each element, in our case the elements are the items in a 
    /// listbox
    /// What this class simply does is compute and assign a size to each item
    /// element in the list box.
    /// In order to have full control of the layout we 
    /// override the MeasureOverride and ArrangeOverride, each called one 
    /// after the order in the given order.
    /// </remarks>
    public class SplitPanel : Panel
    {
        /// <summary>
        /// This is the first of the two overriden methods to be called
        /// In this method we specify the size for each item in the listbox
        /// </summary>
        /// <param name="availableSize"> The available size for the listbox
        /// That is the parent container for the items, this is the space each
        /// child of the listbox has to share.</param>
        /// <returns>
        /// In our case it returns the size (height and width) </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // the final measure size is the available size for the width,
            // and height for the container children
            Size finalSize = new Size { Width = availableSize.Width };

            if (this.Children.Count != 0)
            {
                //here we divide the available width by the number of child items
                //in our case each child get an equal amount of space
                //example lets say the available width is 4, and we have 4 children, 
                //after this computation each child will have a with of 1 (4/4 = 1).
                availableSize.Width /= ((double)this.Children.Count);
            }
            //iterate through the elements in the listbox
            foreach (var current in this.Children)
            {
                //give the child the available size as computed above
                current.Measure(availableSize);
                Size desiredSize = current.DesiredSize;
                finalSize.Height = Math.Max(finalSize.Height, desiredSize.Height);
            }

            // make sure it will works in design time mode
            if (double.IsPositiveInfinity(finalSize.Height) || double.IsPositiveInfinity(finalSize.Width))
            {
                return Size.Empty;
            }

            return finalSize;
        }

        /// <summary>
        /// This is the second (last) of the two overriden methods to be called
        /// In this method we specify how each item is to be positioned in the listbox
        /// </summary>
        /// <param name="arrangeSize"> The size for the listbox
        /// That is the parent container for the items, this is the space each child of the listbox has to share.</param>
        /// <returns>
        /// In our case it return the size </returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //create a rectangle the size of the listbox container
            Rect finalRect = new Rect(new Point(), arrangeSize);
            
            double width = arrangeSize.Width / (this.Children.Count);
            
            //divide the rectangle into the number of children
            //each having equal width as computed in the code statment above
            foreach (var child in this.Children)
            {
                finalRect.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height);
                finalRect.Width = width;

                child.Arrange(finalRect);

                // move each child by the width increment 
                finalRect.X += width;
            }
            return arrangeSize;
        }
    }
}
