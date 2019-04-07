using System.Collections.Generic;

namespace ContosoUniversity.Models.SchoolViewModels
{
    public class DropDownListVM
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class DropDownListVMComparer : IEqualityComparer<DropDownListVM>
    {
        public bool Equals(DropDownListVM x, DropDownListVM y)
        {
            return string.Equals(x.Key, y.Key);
        }

        public int GetHashCode(DropDownListVM obj)
        {
            return obj.Key.GetHashCode();
        }
    }

    public class CheckBoxListVM : DropDownListVM
    {
        public bool IsChecked { get; set; }
    }

}
