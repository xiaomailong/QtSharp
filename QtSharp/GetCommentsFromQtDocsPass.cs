﻿using System;
using System.Collections.Generic;
using System.Linq;
using CppSharp.AST;
using CppSharp.Passes;
using Type = CppSharp.AST.Type;

namespace QtSharp
{
    public class GetCommentsFromQtDocsPass : TranslationUnitPass
    {
        private readonly Documentation documentation;

        public GetCommentsFromQtDocsPass(string docsPath, string module, Dictionary<Type, List<TypedefDecl>> typeDefsPerType)
        {
            this.documentation = new Documentation(docsPath, module, typeDefsPerType);
        }

        public override bool VisitClassDecl(Class @class)
        {
            if (@class.Comment == null)
            {
                this.documentation.DocumentType(@class);
            }
            return base.VisitClassDecl(@class);
        }

        public override bool VisitEnumDecl(Enumeration @enum)
        {
            if (@enum.Comment == null)
            {
                this.documentation.DocumentEnum(@enum);
                foreach (Enumeration.Item item in @enum.Items)
                {
                    this.documentation.DocumentEnumItem(@enum, item);
                }
            }
            return base.VisitEnumDecl(@enum);
        }

        public override bool VisitFunctionDecl(Function function)
        {
            if (function.Comment == null && !function.ExplicityIgnored)
            {
                this.documentation.DocumentFunction(function);
            }
            return base.VisitFunctionDecl(function);
        }

        public override bool VisitProperty(Property property)
        {
            if (property.Comment == null)
            {
                this.documentation.DocumentProperty(property);
            }
            return base.VisitProperty(property);
        }
    }
}