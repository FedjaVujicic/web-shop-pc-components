export class Gpu {
    id: number = 0;
    name: string;
    price: number;
    availability: boolean;
    quantity: number;
    slot: string;
    memory: number;
    ports: Array<string> = [];
    imageName: string = "";
    imageFile: any = null;
};