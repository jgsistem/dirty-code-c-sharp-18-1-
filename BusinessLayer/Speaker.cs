using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;


namespace BusinessLayer
{
   
    public class Speaker
	{

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Exp { get; set; }
        public bool HasBlog { get; set; }
        public string BlogURL { get; set; }
        public BusinessLayer.WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; set; }
        public List<BusinessLayer.Session> Sessions { get; set; }

	
		public int? Register(IRepository repository)
		{
			
			int? speakerId = null;
			bool good = false;
			bool appr = false;
           

			var DatosPersonal = new List<string>() { "Cobol", "Punch Cards", "Commodore", "VBScript" };
            			
			var domains = new List<string>() { "aol.com", "hotmail.com", "prodigy.com", "CompuServe.com" };

            if (!string.IsNullOrWhiteSpace(FirstName))
			{
                if (!string.IsNullOrWhiteSpace(LastName))
				{
                    if (!string.IsNullOrWhiteSpace(Email))
					{						
						var emps = new List<string>() { "Microsoft", "Google", "Fog Creek Software", "37Signals" };
                        good = ((Exp > Int32.Parse(ConfigurationManager.AppSettings["Exp10"]) || HasBlog || Certifications.Count() > Int32.Parse(ConfigurationManager.AppSettings["Exp3"]) || emps.Contains(Employer)));
						if (!good)
						{
							string emailDomain = Email.Split('@').Last();
							if (!domains.Contains(emailDomain) && (!(Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9)))
							{
								good = true;
							}
						}

						if (good)
						{
							if (Sessions.Count() != 0)
							{
								foreach (var session in Sessions)
								{
                                    foreach (var tech in DatosPersonal)
									{
										if (session.Title.Contains(tech) || session.Description.Contains(tech))
										{
											session.Approved = false;
											break;
										}
										else
										{
											session.Approved = true;
											appr = true;
										}
									}
								}
							}
							else
							{
								throw new ArgumentException("Can't register speaker with no sessions to present.");
							}

							if (appr)
							{
                                if (Exp <= Int32.Parse(ConfigurationManager.AppSettings["Exp1"]))
								{
                                    RegistrationFee = Int32.Parse(ConfigurationManager.AppSettings["Exp500"]);
								}
                                else if (Exp >= Int32.Parse(ConfigurationManager.AppSettings["Exp2"]) && Exp <= Int32.Parse(ConfigurationManager.AppSettings["Exp3"]))
								{
                                    RegistrationFee = Int32.Parse(ConfigurationManager.AppSettings["250"]);
								}
                                else if (Exp >= Int32.Parse(ConfigurationManager.AppSettings["Exp4"]) && Exp <= Int32.Parse(ConfigurationManager.AppSettings["Exp5"]))
								{
                                    RegistrationFee = Int32.Parse(ConfigurationManager.AppSettings["Exp500"]);
								}
                                else if (Exp >= Int32.Parse(ConfigurationManager.AppSettings["Exp6"]) && Exp <= Int32.Parse(ConfigurationManager.AppSettings["Exp9"]))
								{
                                    RegistrationFee = Int32.Parse(ConfigurationManager.AppSettings["Exp50"]);
								}
								else
								{
                                    RegistrationFee = Int32.Parse(ConfigurationManager.AppSettings["Exp0"]);
								}
								try
								{
									speakerId = repository.SaveSpeaker(this);
								}
								catch (Exception e)
								{
									
								}
							}
							else
							{
								throw new NoSessionsApprovedException("No sessions approved.");
							}
						}
						else
						{
							throw new SpeakerDoesntMeetRequirementsException("Speaker doesn't meet our abitrary and capricious standards.");
						}
					}
					else
					{
						throw new ArgumentNullException("Email is required.");
					}
				}
				else
				{
					throw new ArgumentNullException("Last name is required.");
				}
			}
			else
			{
				throw new ArgumentNullException("First Name is required");
			}

			return speakerId;
		}

		#region Custom Exceptions
		public class SpeakerDoesntMeetRequirementsException : Exception
		{
			public SpeakerDoesntMeetRequirementsException(string message)
				: base(message)
			{
			}

			public SpeakerDoesntMeetRequirementsException(string format, params object[] args)
				: base(string.Format(format, args)) { }
		}

		public class NoSessionsApprovedException : Exception
		{
			public NoSessionsApprovedException(string message)
				: base(message)
			{
			}
		}
		#endregion
	}
}