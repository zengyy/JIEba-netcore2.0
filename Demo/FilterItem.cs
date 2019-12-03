namespace Demo
{
    public class FilterItem
    {
        public string Field { get; set; }
        /// <summary>
        /// 匹配运算符
        /// =:等于,不区分大小写
        /// ==:等于,区分大小写
        /// like:模糊匹配
        /// </summary>
        public string Oper { get; set; }
        public object Value { get; set; }
    }
}
