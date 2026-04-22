import React, { useMemo } from 'react';
import { useLoaderData, Link } from 'react-router';
import { TaskSummaryDto } from '../types';
import { Card, CardContent } from '@/components/ui/card';
import { Checkbox } from '@/components/ui/checkbox';
import { Button } from '@/components/ui/button';
import { taskService } from '../services/task.service';
import { Plus, Eye, Edit2, Trash2 } from 'lucide-react';
import { useLingui } from '@lingui/react/macro';
import { HasPermission } from '../components/shared/HasPermission';

export const TaskListPage = React.memo(() => {
  const { t } = useLingui();
  const initialTasks = useLoaderData() as TaskSummaryDto[];
  // Note: In a real app, we'd use useObservable(taskService.tasks$, initialTasks)
  // to stay reactive to SignalR or other updates.
  const [tasks, setTasks] = React.useState(initialTasks);

  const handleToggle = (id: number, currentStatus: boolean) => {
    // Optimistic update
    setTasks(prev => prev.map(t => t.id === id ? { ...t, isCompleted: !currentStatus } : t));
    
    // We fetch the full task first because updateTask expects UpdateTaskDto
    // In a real generic CRUD, we might have a specific Toggle endpoint or use JSON Patch
    taskService.getTask(id).subscribe(fullTask => {
      taskService.updateTask(id, {
        title: fullTask.title,
        description: fullTask.description,
        isCompleted: !currentStatus
      }).subscribe();
    });
  };

  const handleDelete = (id: number) => {
    if (confirm(t({ id: 'task.list.delete_confirm', message: 'Are you sure you want to delete this task?' }))) {
      taskService.deleteTask(id).subscribe(() => {
        setTasks(prev => prev.filter(t => t.id !== id));
      });
    }
  };

  const taskList = useMemo(() => (
    <div className="grid gap-4">
      {tasks.length === 0 ? (
        <Card className="border-dashed">
          <CardContent className="flex flex-col items-center justify-center py-12 text-muted-foreground">
            <p>{t({ id: 'task.list.empty', message: 'No tasks found. Create one to get started!' })}</p>
          </CardContent>
        </Card>
      ) : (
        tasks.map((task) => (
          <Card key={task.id} className="group hover:shadow-md transition-shadow">
            <CardContent className="p-4 flex items-center justify-between">
              <div className="flex items-center gap-4">
                <HasPermission permission="Tasks.Update">
                  <Checkbox
                    checked={task.isCompleted}
                    onCheckedChange={() => handleToggle(task.id, task.isCompleted)}
                  />
                </HasPermission>
                <span className={task.isCompleted ? 'line-through text-muted-foreground' : 'font-medium'}>
                  {task.title}
                </span>
              </div>
              <div className="flex items-center gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                <Button variant="ghost" size="icon" asChild>
                  <Link to={`/tasks/${task.id}`}>
                    <Eye className="h-4 w-4" />
                  </Link>
                </Button>
                <HasPermission permission="Tasks.Update">
                  <Button variant="ghost" size="icon" asChild>
                    <Link to={`/tasks/${task.id}/edit`}>
                      <Edit2 className="h-4 w-4" />
                    </Link>
                  </Button>
                </HasPermission>
                <HasPermission permission="Tasks.Delete">
                  <Button variant="ghost" size="icon" onClick={() => handleDelete(task.id)}>
                    <Trash2 className="h-4 w-4 text-destructive" />
                  </Button>
                </HasPermission>
              </div>
            </CardContent>
          </Card>
        ))
      )}
    </div>
  ), [tasks, t]);

  return (
    <div className="max-w-4xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold tracking-tight">{t({ id: 'task.list.title', message: 'Tasks' })}</h1>
          <p className="text-muted-foreground">
            {t({ id: 'task.list.subtitle', message: 'Manage your personal tasks and assignments.' })}
          </p>
        </div>
        <HasPermission permission="Tasks.Create">
          <Button asChild>
            <Link to="/tasks/new">
              <Plus className="h-4 w-4 mr-2" />
              {t({ id: 'task.list.new_task', message: 'New Task' })}
            </Link>
          </Button>
        </HasPermission>
      </div>

      {taskList}
    </div>
  );
});

TaskListPage.displayName = 'TaskListPage';

