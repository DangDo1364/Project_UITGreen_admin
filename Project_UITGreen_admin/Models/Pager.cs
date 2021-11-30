using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class Pager
    {
        public int totalItem { set; get; }
        public int currentPage { set; get; }
        public int pageSize { set; get; }
        public int totalPage { set; get; }
        public int startPage { set; get; }
        public int endPage { set; get; }

        public Pager()
        {
        }
        public Pager(int total_Item, int page, int page_Size = 5)
        {
            int total_Page = (int)Math.Ceiling((decimal)total_Item / (decimal)page_Size);
            int current_Page = page;

            int start_Page = currentPage - 5;
            int end_Page = currentPage + 4;

            if (start_Page <= 0)
            {
                end_Page = end_Page - (start_Page - 1);
                start_Page = 1;
            }
            if (end_Page > total_Page)
            {
                end_Page = total_Page;
                if (end_Page > 10)
                {
                    start_Page = end_Page - 9;
                }
            }
            totalItem = total_Item;
            currentPage = current_Page;
            pageSize = page_Size;
            totalPage = total_Page;
            startPage = start_Page;
            endPage = end_Page;

        }
    }
}
