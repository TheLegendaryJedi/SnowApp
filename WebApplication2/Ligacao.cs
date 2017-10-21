﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
    public class Ligacao
    {
        public static DirectoryEntry GetDirectoryObject()
        {
            DirectoryEntry oDE;
            oDE = new DirectoryEntry("LDAP://exercito.local", "action-user", "schedule_sim", AuthenticationTypes.Secure);
            return oDE;
        }

        public static DirectoryEntry GetUser(string UserName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;

            deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            SearchResult results = deSearch.FindOne();

            if (!(results == null))
            {
                de = new DirectoryEntry(results.Path, "action-user", "schedule_sim", AuthenticationTypes.Secure);
                return de;
            }
            else
            {
                return null;
            }
        }

        public static bool AutenticaUserAD(string usr, string pwd)
        {
            bool authenticated = false;

            try
            {
                DirectoryEntry entry = new DirectoryEntry("LDAP://exercito.local", usr, pwd);
                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch {}
            return authenticated;
        }

        public static string GetProperty(string PropertyName, string UserName)
        {
            DirectoryEntry de = GetDirectoryObject();
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;

            deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            SearchResult results = deSearch.FindOne();

            if (results.Properties.Contains(PropertyName))
            {
                return results.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
}