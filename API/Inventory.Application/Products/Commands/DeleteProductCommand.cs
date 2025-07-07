﻿using MediatR;

namespace Inventory.Application.Products.Commands;

public record DeleteProductCommand(int Id): IRequest;