﻿using System;
using System.Threading.Tasks;
using Weapsy.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Mvc.Context;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Apps.Text.Domain.Commands;
using Weapsy.Apps.Text.Domain;
using Weapsy.Infrastructure.Commands;

namespace Weapsy.Apps.Text.Api
{
    [Route("api/apps/text/[controller]")]
    public class TextController : BaseAdminController
    {
        private readonly ITextModuleFacade _textFacade;
        private readonly ICommandSender _commandSender;

        public TextController(ITextModuleFacade textFacade,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _textFacade = textFacade;
            _commandSender = commandSender;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var text = _textFacade.GetContent(id);
            if (text == null) return NotFound();
            return Ok(text);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateTextModule model)
        {
            model.SiteId = SiteId;
            model.Id = Guid.NewGuid();
            _commandSender.Send<CreateTextModule, TextModule>(model);
            return Ok(string.Empty);
        }

        [HttpPut]
        public IActionResult Put([FromBody] AddVersion model)
        {
            model.SiteId = SiteId;
            model.VersionId = Guid.NewGuid();
            _commandSender.Send<AddVersion, TextModule>(model);
            return Ok(string.Empty);
        }
    }
}
