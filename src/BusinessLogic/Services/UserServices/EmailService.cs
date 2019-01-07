﻿using BusinessLogic.BusinessModels;
using BusinessLogic.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.UserServices
{
	internal class EmailService : IEmailService
	{
		public void Send(object sender, EmailEventArgs args)
		{
			var smtpClient = new SmtpClient();
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

			var message = new MailMessage("TheCheapestTickets@gmail.com", args.User.Email)
			{
				Subject = subject,
				Body = body
			};
			smtpClient.Send(message);
		}

		private void ruEmail(out string subject, out string body, OrderModel model)
		{
			StringBuilder builder = new StringBuilder();
			decimal amount = 0M;
			var seats = model.PurchasedSeats;

			subject = "Приобритение билетов";
			builder.Append("Благодорим за покупку билетов на нашем сайте. Номер вашего заказа: ").Append(model.Order.Id).
				Append("\r\n Список купленных билетов:\r\n");
			for (int i = 0; i < seats.ToList().Count; i++)
			{
				builder.Append(i + 1).Append(". ").Append("Событие: ").Append(seats[i].Event.Title).Append(". Зона: ")
					.Append(seats[i].Area.Description);
				builder.Append(". Место: ").Append(seats[i].Seat.Row).Append("-").Append(seats[i].Seat.Number);
				builder.Append(". Пройдет:").Append(seats[i].Venue.Name).Append(" в ").Append(seats[i].Layout.Description);
				builder.Append(". Дата:").Append(seats[i].Event.Date.Date.ToString(new CultureInfo("ru"))).Append(", в")
					.Append(seats[i].Event.Date.Date.ToString("HH:mm"));
				builder.Append(". Цена").Append(seats[i].Area.Price).Append("\r\n");
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
				Append("\r\n Спіс набытых квіткоў:\r\n");
			for (int i = 0; i < seats.Count; i++)
			{
				builder.Append(i + 1).Append(". ").Append("Падзея:").Append(seats[i].Event.Title).Append(". Зона: ")
					.Append(seats[i].Area.Description);
				builder.Append(". Месца: ").Append(seats[i].Seat.Row).Append("-").Append(seats[i].Seat.Number);
				builder.Append(". Пройдзе:").Append(seats[i].Venue.Name).Append(" у ").Append(seats[i].Layout.Description);
				builder.Append(". Дата:").Append(seats[i].Event.Date.Date.ToString(new CultureInfo("be"))).Append(", у")
					.Append(seats[i].Event.Date.Date.ToString("HH:mm"));
				builder.Append(". Цена").Append(seats[i].Area.Price).Append("\r\n");
				amount += seats[i].Area.Price;
			}
			builder.Append("Агульная сума заказу: ").Append("$").Append(amount.ToString("N2", new CultureInfo("be")));

			body = builder.ToString();
		}

		private void enEmail(out string subject, out string body, OrderModel model)
		{
			StringBuilder builder = new StringBuilder();
			decimal amount = 0M;
			var seats = model.PurchasedSeats;

			subject = "Ticket purchase";
			builder.Append("Thank you for purchasing tickets on our website. Your order number is: ").Append(model.Order.Id).
				Append("\r\n A list of puchased tickets:\r\n");
			for (int i = 0; i < seats.Count; i++)
			{
				builder.Append(i + 1).Append(". ").Append("Event:").Append(seats[i].Event.Title).Append(". Area: ")
					.Append(seats[i].Area.Description);
				builder.Append(". Seat: ").Append(seats[i].Seat.Row).Append("-").Append(seats[i].Seat.Number);
				builder.Append(". Venue:").Append(seats[i].Venue.Name).Append(" in ").Append(seats[i].Layout.Description);
				builder.Append(". Date:").Append(seats[i].Event.Date.Date.ToString(new CultureInfo("en"))).Append(", at")
					.Append(seats[i].Event.Date.Date.ToString("hh:mm tt"));
				builder.Append(". Price").Append(seats[i].Area.Price).Append("\r\n");
				amount += seats[i].Area.Price;
			}
			builder.Append("Total amount: ").Append("$").Append(amount.ToString("N2", new CultureInfo("en")));

			body = builder.ToString();
		}
	}

}
