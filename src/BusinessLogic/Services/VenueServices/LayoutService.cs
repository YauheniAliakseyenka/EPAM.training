using BusinessLogic.DTO;
using BusinessLogic.Exceptions;
using BusinessLogic.Exceptions.VenueExceptions;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace BusinessLogic.Services
{
	internal class LayoutService : IStoreService<LayoutDto, int> 
	{
		private readonly IWorkUnit _context;
		private readonly IStoreService<AreaDto, int> _areaService;

		public LayoutService(IWorkUnit context, IStoreService<AreaDto, int> areaService)
		{
			_context = context;
			_areaService = areaService;
		}

		public void Create(LayoutDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.VenueId == 0)
				throw new LayoutException("VenueID equals zero");

			if (IsDescriptionNotUnique(entity.Description))
				throw new LayoutException("Layout description isn't unique");

			if (entity.AreaList == null || !entity.AreaList.Any())
				throw new LayoutException("Incorrect state of the layout. The layout must have at least one area");

			var layoutAdd = mapToLayout(entity);
			using (var transaction = CustomTransactionScope.GetTransactionScope())
			{
				_context.LayoutRepository.Create(layoutAdd);
				_context.Save();
				entity.Id = layoutAdd.Id;
				foreach (var area in entity.AreaList)
				{
					area.LayoutId = layoutAdd.Id;
					_areaService.Create(area);
				}
				transaction.Complete();
			}
		}

		public void Delete(int id)
		{
			_context.LayoutRepository.Delete(id);
			_context.Save();
		}

		public IEnumerable<LayoutDto> FindBy(Expression<Func<LayoutDto, bool>> expression)
		{
			var result = new List<LayoutDto>();
			Expression<Func<Layout, bool>> predicate = x => expression.Compile().Invoke(mapToLayoutDto(x));
			var list = _context.LayoutRepository.FindBy(predicate).ToList();

			if (!list.Any())
				return result;

			list.ForEach(x =>
			{
				result.Add(mapToLayoutDto(x));
			});

			return result;
		}

		public LayoutDto Get(int id)
		{
			var layout = _context.LayoutRepository.Get(id);

			if (layout == null)
				return null;

			return mapToLayoutDto(layout);
		}

		public  IEnumerable<LayoutDto> GetList()
		{
			var temp = _context.LayoutRepository.GetList();

			if (temp == null)
				return null;

			var result = new List<LayoutDto>();

			temp.ToList().ForEach(x =>
			{
				result.Add(mapToLayoutDto(x));
			});

			return result;
		}

		public void Update(LayoutDto entity)
		{
			if (entity == null)
				throw new ArgumentNullException();

			if (entity.VenueId == 0)
				throw new LayoutException("VenueId equals zero");

			if (IsDescriptionNotUnique(entity.Description))
				throw new LayoutException("Area description isn't unique");

			var update = _context.LayoutRepository.Get(entity.Id);
			update.VenueId = entity.VenueId;
			update.Description = entity.Description;
			_context.LayoutRepository.Update(update);
			_context.Save();
		}

		private LayoutDto mapToLayoutDto(Layout from)
		{
			return new LayoutDto
			{
				AreaList = new List<AreaDto>(),
				Description = from.Description,
				Id = from.Id,
				VenueId = from.VenueId
			};
		}

		private Layout mapToLayout(LayoutDto from)
		{
			return new Layout
			{
				Description = from.Description,
				Id = from.Id,
				VenueId = from.VenueId
			};
		}

		private bool IsDescriptionNotUnique(string description)
		{
			return _context.LayoutRepository.FindBy(x => x.Description.Equals(description, StringComparison.OrdinalIgnoreCase)).Any();
		}
	}
}
