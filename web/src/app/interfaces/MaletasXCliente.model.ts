import { Users } from './Users.model';
import { Bags } from './Bags.model';

export interface MaletasXCliente {
    maletas: Bags[];
    usuario: Users;
}
