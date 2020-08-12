import APIServer from "./APIServer";
import { Request, Response } from "express";

const server = new APIServer();

class APIRoutes {
    @route("get", "/")
    public indexRoute(req: Request, res: Response) {
        return {
            "Hello": "World"
        }
    }
}

function route(method:string, path: string): MethodDecorator {
    return function (target:any, propertyKey: string, descriptor: PropertyDescriptor) {
        server.app[method](path, (req: Request, res: Response) => {
            res.status(200).json(descriptor.value(req, res));
        })
    }
}

server.start();