﻿using KSheet3.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace KSheet3.Components.Pages
{
	public partial class Home
	{
		public bool ShowCreate { get; set; }
		public bool EditRecord { get; set; }
		public int CallId { get; set; }
		public Call? NewCall { get; set; }
		public List<Call>? AllCalls { get; set; }
		
		public Call? CallToUpdate { get; set; }
		private CallContext? _context;

		protected override void OnInitialized()
		{
			ShowCreate = false;
		}

		public void ShowCreateForm()
		{
			ShowCreate = true;
			NewCall = new Call();
		}

		//CREATE
		public async Task CreateNewCall()
		{
			_context ??= await CallContextFactory.CreateDbContextAsync();
			if(NewCall != null)
			{
				NewCall.Time = DateTime.Now;


				if(!string.IsNullOrEmpty(NewCall.Address))
				{
					NewCall.Address = NewCall.Address.ToUpper();
				}
				else
				{
					NewCall.Address = "N/A";
				}

				if(!string.IsNullOrEmpty(NewCall.PdSignal))
				{
					NewCall.PdSignal = NewCall.PdSignal.ToUpper();
				}
				else
				{
					NewCall.PdSignal = "N/A";
				}

				if(!string.IsNullOrEmpty(NewCall.FdSignal))
				{
					NewCall.FdSignal = NewCall.FdSignal.ToUpper();
				}
				else
				{
					NewCall.FdSignal = "N/A";
				}

				_context?.Calls.Add(NewCall);
				_context?.SaveChangesAsync();

			}
			ShowCreate = false;
			await ShowCalls();
		}
		//READ
		public async Task ShowCalls()
		{
			_context ??= await CallContextFactory.CreateDbContextAsync();

			if(_context != null)
			{
				AllCalls = await _context.Calls.ToListAsync();
				AllCalls.Reverse();
			}

		}

		//Shows only a handful of calls when requested
		public async Task ShowSomeCalls()
		{
			_context ??= await CallContextFactory.CreateDbContextAsync();

			if(_context != null)
			{
				AllCalls = await _context.Calls.Take<Call>(20).ToListAsync();
			}

		}

		//UPDATE

		public async Task ShowEditForm(Call call)
		{
			_context ??= await CallContextFactory.CreateDbContextAsync();
			if(_context != null)
			{
				CallToUpdate = _context.Calls.FirstOrDefault(x => x.Id == call.Id);
				CallId = call.Id;
				EditRecord = true;
			}

		}
		public async Task UpdateCall()
		{
			EditRecord = false;
			_context ??= await CallContextFactory.CreateDbContextAsync();

			if(_context != null)
			{
				if(CallToUpdate != null) { _context.Calls.Update(CallToUpdate); }
				await _context.SaveChangesAsync();
			}

		}

		//DELETE
		public async Task DeleteCall(Call call)
		{
			_context ??= await CallContextFactory.CreateDbContextAsync();

			if(_context != null)
			{
				if(call != null)
				{
					_context.Calls.Remove(call);
					await _context.SaveChangesAsync();
				}

				await ShowCalls();
			}
		}
	}
}
