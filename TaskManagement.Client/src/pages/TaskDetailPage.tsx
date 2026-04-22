import React, { useMemo } from 'react';
import { useLoaderData, Link } from 'react-router';
import { TaskDto } from '../types';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Tooltip, TooltipTrigger, TooltipContent } from '@/components/ui/tooltip';
import { ArrowLeft, Edit2, Calendar, User, Clock, CheckCircle2, Circle } from 'lucide-react';

export const TaskDetailPage = React.memo(() => {
  const task = useLoaderData() as TaskDto;

  const formatDate = (dateString?: string) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString(undefined, {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const formatFullDate = (dateString?: string) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleString();
  };

  const details = useMemo(() => (
    <Card>
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <div className="space-y-1">
          <div className="flex items-center gap-2">
            {task.isCompleted ? (
              <CheckCircle2 className="h-5 w-5 text-green-500" />
            ) : (
              <Circle className="h-5 w-5 text-muted-foreground" />
            )}
            <CardTitle className="text-2xl font-bold">{task.title}</CardTitle>
          </div>
          <p className="text-sm text-muted-foreground">Task ID: #{task.id}</p>
        </div>
        <Button variant="outline" size="sm" asChild>
          <Link to={`/tasks/${task.id}/edit`}>
            <Edit2 className="h-4 w-4 mr-2" />
            Edit Task
          </Link>
        </Button>
      </CardHeader>
      <CardContent className="space-y-6 pt-6">
        <div className="space-y-2">
          <h4 className="text-sm font-medium text-muted-foreground uppercase tracking-wider">Description</h4>
          <p className="text-lg leading-relaxed">
            {task.description || <span className="italic text-muted-foreground">No description provided.</span>}
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 pt-6 border-t">
          <div className="flex items-center gap-3">
            <div className="p-2 bg-primary/10 rounded-full">
              <User className="h-5 w-5 text-primary" />
            </div>
            <div>
              <p className="text-sm font-medium text-muted-foreground">Creator</p>
              <Tooltip>
                <TooltipTrigger render={<p className="font-semibold cursor-help" />}>
                  {task.creatorId}
                </TooltipTrigger>
                <TooltipContent>
                  Internal User ID: {task.creatorId}
                </TooltipContent>
              </Tooltip>
            </div>
          </div>

          <div className="flex items-center gap-3">
            <div className="p-2 bg-primary/10 rounded-full">
              <Clock className="h-5 w-5 text-primary" />
            </div>
            <div>
              <p className="text-sm font-medium text-muted-foreground">Created At</p>
              <Tooltip>
                <TooltipTrigger render={<p className="font-semibold cursor-help" />}>
                  {formatDate(task.createdAt)}
                </TooltipTrigger>
                <TooltipContent>
                  Exact time: {formatFullDate(task.createdAt)}
                </TooltipContent>
              </Tooltip>
            </div>
          </div>

          {task.updatedAt && (
            <div className="flex items-center gap-3">
              <div className="p-2 bg-primary/10 rounded-full">
                <Calendar className="h-5 w-5 text-primary" />
              </div>
              <div>
                <p className="text-sm font-medium text-muted-foreground">Last Updated</p>
                <Tooltip>
                  <TooltipTrigger render={<p className="font-semibold cursor-help" />}>
                    {formatDate(task.updatedAt)}
                  </TooltipTrigger>
                  <TooltipContent>
                    Exact time: {formatFullDate(task.updatedAt)}
                  </TooltipContent>
                </Tooltip>
              </div>
            </div>
          )}
        </div>
      </CardContent>
    </Card>
  ), [task]);

  return (
    <div className="max-w-3xl mx-auto space-y-6">
      <Button variant="ghost" size="sm" asChild className="-ml-2">
        <Link to="/tasks">
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back to Tasks
        </Link>
      </Button>

      {details}
    </div>
  );
});

TaskDetailPage.displayName = 'TaskDetailPage';
