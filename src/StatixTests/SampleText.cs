using System;
using System.Collections.Generic;
using System.Text;

namespace StatixTests
{
    public static class SampleText
    {
        public static readonly string SampleMarkdown1 = @"---
tiTlE:   sample title  
dEsCrIption:     sample description  
dAtE:      1985-09-24 01:23:45   
tAgS:  tag1,    tag2,   tag3   
---
other **markdown** content
title:   IGNORE THIS
not in the header";

        public static readonly string SampleTemplate1 = @"
<body>
<div>title={{title}}</div>
<div>description={{description}}</div>
<div>date={{date}}</div>
<div>tags={{tags}}</div>
<div>content={{content}}</div>
</body>
";
    }
}
