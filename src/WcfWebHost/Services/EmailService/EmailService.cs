using BusinessLogic.BusinessModels;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.ServiceModel;
using System.Text;
using WcfBusinessLogic.Core.Contracts.Exceptions;

namespace WcfWebHost.Services.EmailService
{
	internal class EmailService : IEmailService
	{
		public void Send(object sender, OrderEventArgs args)
		{
			var smtp = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
			var pickupDirectoryPath = smtp.SpecifiedPickupDirectory.PickupDirectoryLocation;

			try
			{
				if (!Directory.Exists(pickupDirectoryPath))
					Directory.CreateDirectory(pickupDirectoryPath);
			}
			catch(Exception)
			{
				var fault = new ServiceValidationFaultDetails { Message = "Email service error" };
				throw new FaultException<ServiceValidationFaultDetails>(fault, "Path of specifiedpickupdirectory is invalid");
			}
		
			string subject = string.Empty;
			string body = string.Empty;	

			switch (args.User.Culture)
			{
				case "ru":
					ruEmail(out subject, out body, args.OrderModel);
					break;
				case "be":
					beEmail(out subject, out body, args.OrderModel);
					break;
				case "en":
					enEmail(out subject, out body, args.OrderModel);
					break;
			}

			//Fetching Email Body Text from EmailTemplate File. 
			string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath +
				"Services\\EmailService\\EmailTemplate\\Order.html";
			StreamReader str = new StreamReader(path);
			string mailText = str.ReadToEnd();
			str.Close();

			string imagePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath +
			"Services\\EmailService\\EmailTemplate\\purchase.jpg";
			LinkedResource image = new LinkedResource(imagePath);
			image.ContentId = Guid.NewGuid().ToString();
			mailText = mailText.Replace("[info]", body);
			mailText = mailText.Replace("[image]", "cid:" + image.ContentId);
			AlternateView alternateView = AlternateView.CreateAlternateViewFromString(mailText, null, MediaTypeNames.Text.Html);
			alternateView.LinkedResources.Add(image);

			var message = new MailMessage
			{
				Subject = subject,
				HeadersEncoding = Encoding.UTF8,
				SubjectEncoding = Encoding.UTF8,
				IsBodyHtml = true
			};
			message.AlternateViews.Add(alternateView);
			message.From = new MailAddress(smtp.Network.UserName, "the cheapest tickets");
			message.To.Add(new MailAddress(args.User.Email));

			var smtpClient = new SmtpClient();
			smtpClient.Host = smtp.Network.Host;
			smtpClient.Port = smtp.Network.Port;
			NetworkCredential networkCred = new NetworkCredential(smtp.Network.UserName, smtp.Network.Password);
			smtpClient.UseDefaultCredentials = true;
			smtpClient.Credentials = networkCred;
			smtpClient.Send(message);
		}

		private void ruEmail(out string subject, out string body, OrderModel model)
		{
			StringBuilder builder = new StringBuilder();
			decimal amount = 0M;
			var seats = model.PurchasedSeats;

			subject = "Приобритение билетов";
			builder.Append("Благодорим за покупку билетов на нашем сайте. Номер вашего заказа: ").Append(model.Order.Id).
				 Append("<br />").Append("Список приобретенных билетов:").Append("<br />");
			for (int i = 0; i < seats.ToList().Count; i++)
			{
				builder.Append(i + 1).Append(". ").Append("Событие: ").Append(seats[i].Event.Title).Append(". Зона: ")
					.Append(seats[i].Area.Description);
				builder.Append(". Место: ").Append(seats[i].Seat.Row).Append("-").Append(seats[i].Seat.Number);
				builder.Append(". Пройдет: ").Append(seats[i].Venue.Name).Append(" в ").Append(seats[i].Layout.Description);
				builder.Append(". Дата: ").Append(seats[i].Event.Date.Date.ToShortDateString()).Append(", в ")
					.Append(seats[i].Event.Date.Date.ToString("HH:mm"));
				builder.Append(". Цена $").Append(seats[i].Area.Price.ToString("N2", new CultureInfo("ru"))).Append("<br />");
				amount += seats[i].Area.Price;
			}
			builder.Append("Общая сумма заказа: ").Append("$").Append(amount.ToString("N2", new CultureInfo("ru")));

			body = builder.ToString();
		}

		private void beEmail(out string subject, out string body, OrderModel model)
		{
			StringBuilder builder = new StringBuilder();
			decimal amount = 0M;
			var seats = model.PurchasedSeats;

			subject = "Набыццё білетаў";
			builder.Append("Дзякуем за куплю квіткоў на нашым сайце. Нумар вашага заказ: ").Append(model.Order.Id).
				Append("<br />").Append("Спіс набытых квіткоў:").Append("<br />");
			for (int i = 0; i < seats.Count; i++)
			{
				builder.Append(i + 1).Append(". ").Append("Падзея: ").Append(seats[i].Event.Title).Append(". Зона: ")
					.Append(seats[i].Area.Description);
				builder.Append(". Месца: ").Append(seats[i].Seat.Row).Append("-").Append(seats[i].Seat.Number);
				builder.Append(". Пройдзе: ").Append(seats[i].Venue.Name).Append(" у ").Append(seats[i].Layout.Description);
				builder.Append(". Дата: ").Append(seats[i].Event.Date.Date.ToShortDateString()).Append(", у ")
					.Append(seats[i].Event.Date.Date.ToString("HH:mm"));
				builder.Append(". Цана $").Append(seats[i].Area.Price).Append("<br />");
				amount += seats[i].Area.Price;
			}
			builder.Append("Агульная сума заказу: $").Append(amount.ToString("N2", new CultureInfo("be")));

			body = builder.ToString();
		}

		private void enEmail(out string subject, out string body, OrderModel model)
		{
			StringBuilder builder = new StringBuilder();
			decimal amount = 0M;
			var seats = model.PurchasedSeats;

			subject = "Ticket purchase";
			builder.Append("Thank you for purchasing tickets on our website. Your order number is: ").Append(model.Order.Id).
				Append("<br />").Append("A list of puchased tickets:").Append("<br />");
			for (int i = 0; i < seats.Count; i++)
			{
				builder.Append(i + 1).Append(". ").Append("Event: ").Append(seats[i].Event.Title).Append(". Area: ")
					.Append(seats[i].Area.Description);
				builder.Append(". Seat: ").Append(seats[i].Seat.Row).Append("-").Append(seats[i].Seat.Number);
				builder.Append(". Venue: ").Append(seats[i].Venue.Name).Append(" in ").Append(seats[i].Layout.Description);
				builder.Append(". Date: ").Append(seats[i].Event.Date.Date.ToShortDateString()).Append(", at ")
					.Append(seats[i].Event.Date.Date.ToString("hh:mm tt"));
				builder.Append(". Price: $").Append(seats[i].Area.Price).Append("<br />");
				amount += seats[i].Area.Price;
			}
			builder.Append("Total amount: ").Append("$").Append(amount.ToString("N2", new CultureInfo("en")));

			body = builder.ToString();
		}
	}

}
