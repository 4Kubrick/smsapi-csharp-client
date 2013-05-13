﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;

namespace SMSApi.Api.Action
{
    public class VMSSend : Send
    {
        public VMSSend() : base() { }

        public SMSApi.Api.Response.Status Execute()
        {
            Validate();

            Stream data;

            if (File != null && File.Length > 0)
            {
                data = proxy.Execute("vms.do", Values(), File);
            }
            else
            {
                data = proxy.Execute("vms.do", Values());
            }

            var serializer = new DataContractJsonSerializer(typeof(SMSApi.Api.Response.Status));
            SMSApi.Api.Response.Status response = (SMSApi.Api.Response.Status)serializer.ReadObject(data);
            data.Close();

            return response;
        }

        private NameValueCollection Values()
        {
            NameValueCollection collection = new NameValueCollection();

            collection.Add("format", "json");

            collection.Add("username", client.GetUsername());
            collection.Add("password", client.GetPassword());

            if (To != null)
                collection.Add("to", string.Join(",", To));

//            if (Group != null)
//                collection.Add("group", Group);

            if (TTS != null)
                collection.Add("tts", TTS);

            if (DateSent != null)
                collection.Add("date", DateSent);

            if (Partner != null)
                collection.Add("partner_id", Partner);

            if (Test == true)
                collection.Add("test", "1");

            if (Idx != null && Idx.Length > 0)
            {
                collection.Add("check_idx", (IdxCheck ? "1" : "0"));
                collection.Add("idx", string.Join("|", Idx));
            }

            return collection;
        }

        private void Validate()
        {
            if( To != null && Group != null )
            {
                throw new ArgumentException("Cannot use 'to' and 'group' at the same time!");
            }

            if ( (TTS == null || TTS.Length < 1) && (File == null || File.Length == 0) )
            {
                throw new ArgumentException("Cannot send message without content!");
            }

            if (TTS != null  && File != null)
            {
                throw new ArgumentException("Cannot send TTS and file at the same time");
            }
        }

        private Stream File;
        private string TTS;

        public VMSSend SetTo(string to)
        {
            this.To = new string[] { to };
            return this;
        }

        public VMSSend SetTo(string[] to)
        {
            this.To = to;
            return this;
        }

        public VMSSend SetGroup(string group)
        {
            this.Group = group;
            return this;
        }

        public VMSSend SetDateSent(string data)
        {
            this.DateSent = data;
            return this;
        }

        public VMSSend SetDateSent(DateTime data)
        {
            this.DateSent = data.ToString("yyyy-MM-ddTHH:mm:ssK");
            return this;
        }

        public VMSSend SetIDx(string idx)
        {
            this.Idx = new string[] { idx };
            return this;
        }

        public VMSSend SetIDx(string[] idx)
        {
            this.Idx = idx;
            return this;
        }

        public VMSSend SetCheckIDx(bool check = true)
        {
            this.IdxCheck = check;
            return this;
        }

        public VMSSend SetFile(Stream file)
        {
            this.File = file;
            return this;
        }

        public VMSSend SetTTS(string tts)
        {
            this.TTS = tts;
            return this;
        }

        public VMSSend SetPartner(string partner)
        {
            this.Partner = partner;
            return this;
        }

        public VMSSend SetTest(bool test = true)
        {
            this.Test = test;
            return this;
        }
    }
}
