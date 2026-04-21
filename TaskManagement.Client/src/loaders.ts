import { firstValueFrom } from 'rxjs';
import { taskService } from './services/task.service';
import { authService } from './services/auth.service';
import { redirect } from 'react-router';

export const tasksLoader = async () => {
  if (!authService.isAuthenticated) return redirect('/login');
  return await firstValueFrom(taskService.fetchTasks());
};

export const taskDetailLoader = async ({ params }: any) => {
  if (!authService.isAuthenticated) return redirect('/login');
  const id = parseInt(params.id);
  if (isNaN(id)) throw new Error('Invalid Task ID');
  return await firstValueFrom(taskService.getTask(id));
};
