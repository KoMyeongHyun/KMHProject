using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Record : Item
{
    private StringBuilder content;

    public StringBuilder CONTENT
    {
        set { content = value; }
        get { return content; }
    }
    //자료구조 사용해서 xml로 읽어온 내용 끊어서 보관할 것 
    //stringBuilder 사용 검토해볼 것  string + string 사용은 자제
}
